using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleScript.Binding;
using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns;

namespace SimpleScript.Tests
{
    internal class ScriptRunner : IScriptRunner
    {
        private readonly IFunction[] functions;
        private readonly IEnhancedParser parser;
        private readonly IBinder binder;
        private Dictionary<string, object> variables;

        public ScriptRunner(IFunction[] functions, IEnhancedParser parser, IBinder binder)
        {
            this.functions = functions;
            this.parser = parser;
            this.binder = binder;
        }

        public async Task<Either<ErrorList, Success>> Run(string input, Dictionary<string, object> variables)
        {
            this.variables = variables;

            var mapSuccess = parser
                .Parse(input)
                .MapError(pr => new ErrorList(pr.Message))
                .MapSuccess(parsed => binder.Bind(parsed)
                    .MapSuccess(Execute))
                .MapSuccess(async t =>
                {
                    await t;
                    return new Success();
                });

            return await mapSuccess.AwaitRight();
        }

        private Task<Either<ErrorList, Success>> Execute(BoundScript bound)
        {
            return Execute(bound.StartupFunction);
        }

        private Task<Either<ErrorList, Success>> Execute(BoundFunctionDeclaration main)
        {
            return Execute(main.Block);
        }

        private async Task<Either<ErrorList, Success>> Execute(BoundBlock block)
        {
            var asyncSelect = await block.BoundStatements.AsyncSelect(Execute);

            var combine = Either
                .Combine(asyncSelect, Errors.Concat)
                .MapSuccess(successes => new Success());

            return combine;
        }

        private async Task<Either<ErrorList, Success>> Execute(BoundStatement statement)
        {
            switch (statement)
            {
                case BoundAssignmentStatement boundAssignmentStatement:
                    return await Execute(boundAssignmentStatement);
                case BoundCallStatement boundCallStatement:
                    return await Execute(boundCallStatement);
                case BoundEchoStatement boundEchoStatement:
                    break;
                case BoundIfStatement boundIfStatement:
                    return await Execute(boundIfStatement);
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }

            return new Success();
        }

        private async Task<Either<ErrorList, Success>> Execute(BoundCallStatement boundCallStatement)
        {
            var either = await Evaluate(boundCallStatement.Call);
            return either.MapSuccess(o => new Success());
        }

        private async Task<Either<ErrorList, Success>> Execute(BoundIfStatement boundIfStatement)
        {
            var eitherComparison = await IsMet(boundIfStatement.Condition);

            var result = eitherComparison.MapSuccess(async isMet =>
            {
                if (isMet)
                {
                    var executeResult = await Execute(boundIfStatement.TrueBlock);

                    return executeResult;
                }
                else
                {
                    var optionalTask = boundIfStatement.FalseBlock.Map(Execute);
                    var task = (Task<Either<ErrorList, Success>>) optionalTask.Match(t => t, () => Task.CompletedTask);
                    return await task;
                }
            });

            var right = await result.AwaitRight();

            return right.MapSuccess(x => x.MapSuccess(success => success));
        }

        private async Task<Either<ErrorList, bool>> IsMet(BoundCondition condition)
        {
            var left = await Evaluate(condition.Left);
            var right = await Evaluate(condition.Right);
            return Either.Combine(left, right, (x, y) => Compare(x, y, condition.Op), Errors.Concat);
        }

        private Either<ErrorList, bool> Compare(object a, object b, BooleanOperator op)
        {
            if (a is string strA && b is string strB)
            {
                if (op.Op == "==")
                {
                    return strB.Equals(strA);
                }

                if (op.Op == "!=")
                {
                    return !strB.Equals(strA);
                }
            }

            if (a is int x && b is int y)
            {
                if (op.Op == ">")
                {
                    return x > y;
                }

                if (op.Op == "<")
                {
                    return x < y;
                }

                if (op.Op == "==")
                {
                    return x == y;
                }

                if (op.Op == "!=")
                {
                    return x != y;
                }

                if (op.Op == ">=")
                {
                    return x >= y;
                }

                if (op.Op == "<=")
                {
                    return x <= y;
                }
            }

            return new ErrorList($"Cannot compare '{a}' of type {a.GetType()} and '{b}' of type {b.GetType()}");
        }

        private async Task<Either<ErrorList, Success>> Execute(BoundAssignmentStatement boundAssignmentStatement)
        {
            var evaluation = await Evaluate(boundAssignmentStatement.Expression);
            evaluation.WhenRight(o => variables[boundAssignmentStatement.Variable] = o);
            return evaluation.MapSuccess(o => new Success());
        }

        private async Task<Either<ErrorList, object>> Evaluate(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundIdentifier boundIdentifier:
                    return await Evaluate(boundIdentifier);
                case BoundBuiltInFunctionCallExpression boundBuiltInFunctionCallExpression:
                    return await Evaluate(boundBuiltInFunctionCallExpression);
                case BoundStringExpression boundStringExpression:
                    return boundStringExpression.String;
                case BoundCustomCallExpression boundCustomCallExpression:
                    return await Evaluate(boundCustomCallExpression);
                case BoundNumericExpression boundNumericExpression:
                    return boundNumericExpression.Value;
            }

            throw new ArgumentOutOfRangeException(nameof(expression));
        }

        private async Task<Either<ErrorList, object>> Evaluate(BoundCustomCallExpression call)
        {
            return await Execute(call.FunctionDeclaration.Block);
        }

        private async Task<Either<ErrorList, object>> Evaluate(BoundBuiltInFunctionCallExpression call)
        {
            var eitherParameters = await call.Parameters.AsyncSelect(Evaluate);
            var eitherParameter = eitherParameters.Combine(Errors.Concat);

            try
            {
                var mapSuccess = eitherParameter.MapSuccess(async parameters => await call.Function.Invoke(parameters.ToArray()));
                var right = await mapSuccess.AwaitRight();
                return right;
            }
            catch (Exception ex)
            {
                return new ErrorList($"Function failed with exception {ex.Message}");
            }
        }

        private async Task<Either<ErrorList, object>> Evaluate(BoundIdentifier identifier)
        {
            if (variables.TryGetValue(identifier.Identifier, out var val))
            {
                return val;
            }

            return new ErrorList($"Could not find variable '{identifier.Identifier}'");
        }
    }
}