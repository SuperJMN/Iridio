using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Core;
using Zafiro.Core;

namespace Iridio.Runtime
{
    public class ScriptRunner : IScriptRunner
    {
        private readonly IEnumerable<IFunction> functions;
        private IDictionary<string, object> variables;
        private readonly ISubject<string> messages = new Subject<string>();

        public ScriptRunner(IEnumerable<IFunction> functions)
        {
            this.functions = functions;
        }

        public Task<Result<ExecutionSummary, RunError>> Run(Script script)
        {
            return Execute(script);
        }

        public IObservable<string> Messages => messages.AsObservable();

        private async Task<Result<ExecutionSummary, RunError>> Execute(Script script)
        {
            variables = new Dictionary<string, object>();

            return await GetMain(script)
                .Bind(Execute)
                .Map(_ => new ExecutionSummary(variables));
        }

        private Result<BoundProcedure, RunError> GetMain(Script script)
        {
            return script.Procedures
                .TryFirst(x => x.Name == "Main")
                .ToResult((RunError) new MainProcedureNotFound());
        }

        private Task<Result<Success, RunError>> Execute(BoundProcedure main)
        {
            return Execute(main.Block);
        }

        private async Task<Result<Success, RunError>> Execute(BoundBlock block)
        {
            foreach (var st in block.Statements)
            {
                var result = await Execute(st);
                if (result.IsFailure)
                {
                    return result.Error;
                }
            }

            return Success.Value;
        }

        private async Task<Result<Success, RunError>> Execute(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundAssignmentStatement assignment:
                    return await Execute(assignment);
                case BoundCallStatement call:
                    return await Execute(call);
                case BoundEchoStatement echo:
                    messages.OnNext(echo.Message);
                    break;
                case BoundIfStatement ifStatement:
                    return await Execute(ifStatement);
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }

            return Result.Success<Success, RunError>(Success.Value);
        }

        private async Task<Result<Success, RunError>> Execute(BoundCallStatement boundCallStatement)
        {
            var either = await Evaluate(boundCallStatement.Call);
            return either.Bind(o => Result.Success<Success, RunError>(Success.Value));
        }

        private async Task<Result<Success, RunError>> Execute(BoundIfStatement boundIfStatement)
        {
            var eval = await Eval(boundIfStatement.Condition);

            var result = await eval.Bind(async isMet =>
            {
                if (isMet)
                {
                    return await Execute(boundIfStatement.TrueBlock);
                }

                var elseExecution = await boundIfStatement.FalseBlock
                    .Map(async block => await Execute(block));

                return elseExecution.Unwrap(r => Success.Value);
            });

            return result;
        }

        private async Task<Result<bool, RunError>> Eval(BoundExpression condition)
        {
            return (await Evaluate(condition))
                .Bind(o => Result.Success<bool, RunError>((bool) o));
        }

        private async Task<Result<Success, RunError>> Execute(BoundAssignmentStatement boundAssignmentStatement)
        {
            var evaluation = await Evaluate(boundAssignmentStatement.Expression);
            return evaluation
                .Tap(o => variables[boundAssignmentStatement.Variable] = o)
                .Map(o => Success.Value);
        }

        private async Task<Result<object, RunError>> Evaluate(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundBinaryExpression boundBinaryExpression:
                    return await Evaluate(boundBinaryExpression);
                case BoundBooleanValueExpression boundBooleanValueExpression:
                    return Evaluate(boundBooleanValueExpression);
                case BoundIdentifier boundIdentifier:
                    return Evaluate(boundIdentifier);
                case BoundBuiltInFunctionCallExpression boundBuiltInFunctionCallExpression:
                    return await Evaluate(boundBuiltInFunctionCallExpression);
                case BoundStringExpression boundStringExpression:
                    return Evaluate(boundStringExpression);
                case BoundProcedureCallExpression boundCustomCallExpression:
                    return await Evaluate(boundCustomCallExpression);
                case BoundIntegerExpression boundNumericExpression:
                    return Result.Success<object, RunError>(boundNumericExpression.Value);
                case BoundUnaryExpression boundUnaryExpression:
                    return await Evaluate(boundUnaryExpression);
                case BoundDoubleExpression boundDoubleExpression:
                    return Result.Success<object, RunError>(boundDoubleExpression.Value);
            }

            throw new ArgumentOutOfRangeException(nameof(expression));
        }

        private async Task<Result<object, RunError>> Evaluate(BoundUnaryExpression boundUnaryExpression)
        {
            var result = await Evaluate(boundUnaryExpression.Expression);

            return result
                .Map(o => boundUnaryExpression.Op.Calculate(o));
        }

        private Result<object, RunError> Evaluate(BoundBooleanValueExpression expr)
        {
            return expr.Value;
        }

        private async Task<Result<object, RunError>> Evaluate(BoundBinaryExpression boundBinaryExpression)
        {
            var leftEither = await Evaluate(boundBinaryExpression.Left);
            var rightResult = await Evaluate(boundBinaryExpression.Right);

            var result = Result.Combine(e => e.First(), leftEither, rightResult);
            if (result.IsSuccess)
            {
                return boundBinaryExpression.Op.Calculate(leftEither.Value, rightResult.Value);
            }

            return result;
        }

        private Result<object, RunError> Evaluate(BoundStringExpression boundStringExpression)
        {
            var str = boundStringExpression.String;
            var evaluator = new StringEvaluator();
            var either = evaluator.Evaluate(str, variables);
            return either.Check(Result.Success<object, RunError>);
        }

        private async Task<Result<object, RunError>> Evaluate(BoundProcedureCallExpression call)
        {
            var execute = await Execute(call.Procedure.Block);
            return execute.Check(Result.Success<object, RunError>);
        }

        private async Task<Result<object, RunError>> Evaluate(BoundBuiltInFunctionCallExpression call)
        {
            var function = Maybe.From(functions.FirstOrDefault(f => f.Name == call.Function.Name));

            var result = await function
                .ToResult((RunError)new UndeclaredFunction(call.Function.Name, call.Position))
                .Bind(f => Call(f, call.Parameters, call.Position));

            return result;
        }

        private async Task<Result<object, RunError>> Call(IFunction function, IEnumerable<BoundExpression> parameters, Position callPosition)
        {
            var eitherParameters = await parameters.AsyncSelect(async expression => await Evaluate(expression));
            var combined = eitherParameters.Combine(errors => errors.First());

            try
            {
                return await combined.Map(p => function.Invoke(p.ToArray()));
            }
            catch (Exception ex)
            {
                switch (ex.InnerException)
                {
                    case TaskCanceledException tc:
                        return Result.Failure<object, RunError>(new ExecutionCanceled(tc.Message, callPosition));
                    case Exception inner:
                        return Result.Failure<object, RunError>(new IntegratedFunctionFailed(function, inner, callPosition));
                    default:
                        return Result.Failure<object, RunError>(new IntegratedFunctionFailed(function, ex, callPosition));
                }
            }
        }

        private Result<object, RunError> Evaluate(BoundIdentifier identifier)
        {
            if (variables.TryGetValue(identifier.Identifier, out var val))
            {
                return Result.Success<object, RunError>(val);
            }

            return Result.Failure<object, RunError>(new ReferenceToUnsetVariable(identifier.Position, identifier.Identifier));
        }
    }
}