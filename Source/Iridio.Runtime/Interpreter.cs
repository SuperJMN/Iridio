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

namespace Iridio.Runtime
{
    public class Interpreter : IInterpreter
    {
        private readonly IEnumerable<IFunction> functions;
        private IDictionary<string, object> variables;
        private readonly ISubject<string> messages = new Subject<string>();
        private Dictionary<string, BoundProcedure> procedures;

        public Interpreter(IEnumerable<IFunction> functions)
        {
            this.functions = functions;
        }

        public Task<Result<ExecutionSummary, RunError>> Run(Script script)
        {
            procedures = script.Procedures.ToDictionary(x => x.Name, x => x);
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

        private Task<Result<Success, RunError>> Execute(BoundProcedure procedure)
        {
            return Execute(procedure.Block);
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
            var evaluation = await EvaluateExpression(boundCallStatement.Call);
            return evaluation.Map(_ => Success.Value);
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

                return elseExecution.Unwrap(_ => Success.Value);
            });

            return result;
        }

        private async Task<Result<bool, RunError>> Eval(BoundExpression condition)
        {
            return (await EvaluateExpression(condition))
                .Bind(o => Result.Success<bool, RunError>((bool)o));
        }

        private async Task<Result<Success, RunError>> Execute(BoundAssignmentStatement boundAssignmentStatement)
        {
            var evaluation = await EvaluateExpression(boundAssignmentStatement.Expression);
            return evaluation
                .Tap(o => variables[boundAssignmentStatement.Variable] = o)
                .Map(_ => Success.Value);
        }

        private async Task<Result<object, RunError>> EvaluateExpression(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundConstantExpression bce:
                    return EvaluateConstant(bce);
                case BoundUnaryExpression boundUnaryExpression:
                    return await EvaluateUnaryExpression(boundUnaryExpression);
                case BoundBinaryExpression boundBinaryExpression:
                    return await EvaluateBinaryExpression(boundBinaryExpression);
                case BoundReference boundReference:
                    return EvaluateReference(boundReference);
                case BoundFunctionCallExpression boundFunctionCallExpression:
                    return await EvaluateFunction(boundFunctionCallExpression);
                case BoundProcedureCallExpression boundProcedureSymbolCallExpression:
                    return await EvaluateProcedure(boundProcedureSymbolCallExpression);
            }

            throw new ArgumentOutOfRangeException(nameof(expression));
        }

        private Result<object, RunError> EvaluateConstant(BoundConstantExpression bce)
        {
            return bce.Value switch
            {
                double d => d,
                int i => i,
                string str => EvaluateString(str, bce.Position),
                bool b => b,
                _ => throw new InvalidOperationException($"Unsupported constant type {bce.Value.GetType()}")
            };
        }

        private Result<object, RunError> EvaluateString(string str, Maybe<Position> position)
        {
            var evaluator = new StringEvaluator();
            var either = evaluator.Evaluate(str, variables);
            return either.MapError(missing => new ReferenceToUnsetVariable(position, missing.ToArray()));
        }

        private async Task<Result<object, RunError>> EvaluateUnaryExpression(BoundUnaryExpression boundUnaryExpression)
        {
            var result = await EvaluateExpression(boundUnaryExpression.Expression);

            return result
                .Map(o => boundUnaryExpression.Op.Calculate(o));
        }

        private async Task<Result<object, RunError>> EvaluateBinaryExpression(BoundBinaryExpression boundBinaryExpression)
        {
            var leftEither = await EvaluateExpression(boundBinaryExpression.Left);
            var rightResult = await EvaluateExpression(boundBinaryExpression.Right);

            var result = Result.Combine(e => e.First(), leftEither, rightResult);
            if (result.IsSuccess)
            {
                return boundBinaryExpression.Op.Calculate(leftEither.Value, rightResult.Value);
            }

            return result;
        }

        private async Task<Result<object, RunError>> EvaluateProcedure(BoundProcedureCallExpression boundProcedureCallExpression)
        {
            var symbol = boundProcedureCallExpression.ProcedureSymbol;
            var procedure = procedures.TryFind(symbol.Name);
            return await procedure.Map(Execute);
        }

        private async Task<Result<object, RunError>> EvaluateFunction(BoundFunctionCallExpression call)
        {
            var function = Maybe.From(functions.FirstOrDefault(f => f.Name == call.Function.Name));

            var result = await function
                .ToResult((RunError)new UndeclaredFunction(call.Function.Name, call.Position))
                .Bind(f => Call(f, call.Parameters, call.Position));

            return result;
        }

        private async Task<Result<object, RunError>> Call(IFunction function, IEnumerable<BoundExpression> parameters, Maybe<Position> callPosition)
        {
            var eitherParameters = await Task.WhenAll(parameters.Select(EvaluateExpression));
            var combined = eitherParameters.Combine(errors => errors.First());

            try
            {
                return await combined.Map(p => function.Invoke(p.ToArray()));
            }
            catch (Exception ex)
            {
                return ex.InnerException switch
                {
                    TaskCanceledException tc => Result.Failure<object, RunError>(new ExecutionCanceled(tc.Message, callPosition)),
                    { } inner => Result.Failure<object, RunError>(new IntegratedFunctionFailed(function, inner, callPosition)),
                    _ => Result.Failure<object, RunError>(new IntegratedFunctionFailed(function, ex, callPosition))
                };
            }
        }

        private Result<object, RunError> EvaluateReference(BoundReference reference)
        {
            if (variables.TryGetValue(reference.Identifier, out var val))
            {
                return Result.Success<object, RunError>(val);
            }

            return Result.Failure<object, RunError>(new ReferenceToUnsetVariable(reference.Position, reference.Identifier));
        }
    }
}