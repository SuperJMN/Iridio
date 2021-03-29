using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing.Model;
using Optional.Async.Extensions;
using Optional.Collections;
using Optional.Unsafe;
using Zafiro.Core;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    // ReSharper disable once UnusedType.Global
    public class ScriptRunner : IScriptRunner
    {
        private readonly IEnumerable<IFunction> functions;
        private readonly IDictionary<string, object> variables = new Dictionary<string, object>();
        private readonly ISubject<string> messages = new Subject<string>();

        public ScriptRunner(IEnumerable<IFunction> functions)
        {
            this.functions = functions;
        }

        public Task<Either<RunError, ExecutionSummary>> Run(Script script)
        {
            return Execute(script);
        }

        public IObservable<string> Messages => messages.AsObservable();

        public IReadOnlyDictionary<string, object> Variables => new ReadOnlyDictionary<string, object>(variables);

        private async Task<Either<RunError, ExecutionSummary>> Execute(Script compiled)
        {
            var execute = await Execute(compiled.MainProcedure);
            return execute.MapRight(x => new ExecutionSummary(variables));
        }

        private Task<Either<RunError, Success>> Execute(BoundProcedure main)
        {
            return Execute(main.Block);
        }

        private async Task<Either<RunError, Success>> Execute(BoundBlock block)
        {
            foreach (var st in block.Statements)
            {
                var result = await Execute(st);
                if (!result.IsRight())
                {
                    return result.Left.ValueOrFailure();
                }
            }

            return Success.Value;
        }

        private async Task<Either<RunError, Success>> Execute(BoundStatement statement)
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

            return Either.Success<RunError, Success>(Success.Value);
        }

        private async Task<Either<RunError, Success>> Execute(BoundCallStatement boundCallStatement)
        {
            var either = await Evaluate(boundCallStatement.Call);
            return either.MapRight(o => Success.Value);
        }

        private async Task<Either<RunError, Success>> Execute(BoundIfStatement boundIfStatement)
        {
            var eitherComparison = await IsMet(boundIfStatement.Condition);

            var result = eitherComparison.MapRight(async isMet =>
            {
                if (isMet)
                {
                    var executeResult = await Execute(boundIfStatement.TrueBlock);

                    return executeResult;
                }

                var optionalTask = boundIfStatement.FalseBlock.Map(Execute);
                var task = optionalTask.Match(t => t, () => Task.FromResult(Either.Success<RunError, Success>(Success.Value)));
                return await task;
            });

            return await result.RightTask();
        }

        private async Task<Either<RunError, bool>> IsMet(BoundExpression condition)
        {
            return (await Evaluate(condition))
                .MapRight(o => (bool)o);
        }

        private async Task<Either<RunError, Success>> Execute(BoundAssignmentStatement boundAssignmentStatement)
        {
            var evaluation = await Evaluate(boundAssignmentStatement.Expression);
            evaluation.WhenRight(o => variables[boundAssignmentStatement.Variable] = o);
            return evaluation.MapRight(o => Success.Value);
        }

        private async Task<Either<RunError, object>> Evaluate(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundBinaryExpression boundBinaryExpression:
                    return await Evaluate(boundBinaryExpression);
                case BoundBooleanValueExpression boundBooleanValueExpression:
                    return await Evaluate(boundBooleanValueExpression);
                case BoundIdentifier boundIdentifier:
                    return await Evaluate(boundIdentifier);
                case BoundBuiltInFunctionCallExpression boundBuiltInFunctionCallExpression:
                    return await Evaluate(boundBuiltInFunctionCallExpression);
                case BoundStringExpression boundStringExpression:
                    return Evaluate(boundStringExpression);
                case BoundProcedureCallExpression boundCustomCallExpression:
                    return await Evaluate(boundCustomCallExpression);
                case BoundCallExpression boundCallExpression:
                    break;
                case BoundCondition boundCondition:
                    break;
                case BoundIntegerExpression boundNumericExpression:
                    return Either.Success<RunError, object>(boundNumericExpression.Value);
                case BoundUnaryExpression boundUnaryExpression:
                    return await Evaluate(boundUnaryExpression);
                case BoundDoubleExpression boundDoubleExpression:
                    return Either.Success<RunError, object>(boundDoubleExpression.Value);
            }

            throw new ArgumentOutOfRangeException(nameof(expression));
        }

        private async Task<Either<RunError, object>> Evaluate(BoundUnaryExpression boundUnaryExpression)
        {
            return (await Evaluate(boundUnaryExpression.Expression))
                .MapRight(o => boundUnaryExpression.Op.Calculate(o));
        }

        private async Task<Either<RunError, object>> Evaluate(BoundBooleanValueExpression expr)
        {
            return expr.Value;
        }

        private async Task<Either<RunError, object>> Evaluate(BoundBinaryExpression boundBinaryExpression)
        {
            var leftEither = await Evaluate(boundBinaryExpression.Left);
            var esr = await leftEither.MapRight(async a =>
            {
                var rightEither = await Evaluate(boundBinaryExpression.Right);
                return rightEither.MapRight(b => boundBinaryExpression.Op.Calculate(a, b));
            }).RightTask();

            return esr;
        }

        private Func<dynamic, dynamic, object> GetOperation(BinaryOperator op)
        {
            return op.Calculate;
        }

        private Either<RunError, object> Evaluate(BoundStringExpression boundStringExpression)
        {
            var str = boundStringExpression.String;
            var evaluator = new StringEvaluator();
            var either = evaluator.Evaluate(str, variables);
            return either.MapRight(s => (object)s);
        }

        private async Task<Either<RunError, object>> Evaluate(BoundProcedureCallExpression call)
        {
            return (await Execute(call.Procedure.Block)).MapRight(s => (object)s);
        }

        private Task<Either<RunError, object>> Evaluate(BoundBuiltInFunctionCallExpression call)
        {
            var function = OptionCollectionExtensions.FirstOrNone(functions, f => f.Name == call.Function.Name);
            return function.MatchAsync(f => Call(f, call.Parameters), () => Either.Error<RunError, object>(new UndeclaredFunction(call.Function.Name)));
        }

        private async Task<Either<RunError, object>> Call(IFunction function, IEnumerable<BoundExpression> parameters)
        {
            var eitherParameters = await parameters.AsyncSelect(async expression => await Evaluate(expression));
            var combined = eitherParameters.Combine((a, b) => a);

            try
            {
                var mapSuccess = combined.MapRight(async p => await function.Invoke(p.ToArray()));
                var right = await mapSuccess.RightTask();
                return right;
            }
            catch (Exception ex)
            {
                switch (ex.InnerException)
                {
                    case TaskCanceledException tc:
                        return Either.Error<RunError, object>(new ExecutionCanceled(tc.Message));
                    case Exception inner:
                        return Either.Error<RunError, object>(new IntegratedFunctionFailed(function, inner));
                    default:
                        return Either.Error<RunError, object>(new IntegratedFunctionFailed(function, ex));
                }
            }
        }

        private async Task<Either<RunError, object>> Evaluate(BoundIdentifier identifier)
        {
            if (Variables.TryGetValue(identifier.Identifier, out var val))
            {
                return Either.Success<RunError, object>(val);
            }

            return Either.Error<RunError, object>(new ReferenceToUnsetVariable(identifier.Identifier));
        }
    }
}