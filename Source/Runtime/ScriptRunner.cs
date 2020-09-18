using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;
using Optional.Collections;
using SimpleScript;
using SimpleScript.Binding;
using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using SimpleScript.Tests;
using SimpleScript.Tokenization;
using Zafiro.Core;
using Zafiro.Core.Patterns.Either;

namespace Runtime
{
    public class ScriptRunner : IScriptRunner
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

        public async Task<Either<Errors, Success>> Run(string input, Dictionary<string, object> variables)
        {
            this.variables = variables;

            var mapSuccess = parser
                .Parse(input)
                .MapLeft(pr => new Errors(ErrorKind.UnableToParse))
                .MapRight(parsed => binder.Bind(parsed)
                    .MapRight(async bound =>
                    {
                        var execute = await Execute(bound);
                        return execute;
                    }));

            var awaitRight = await mapSuccess.RightTask();
            return awaitRight.MapRight(either => new Success());
        }

        private Task<Either<Errors, Success>> Execute(BoundScript bound)
        {
            return Execute(bound.StartupFunction);
        }

        private Task<Either<Errors, Success>> Execute(BoundFunctionDeclaration main)
        {
            return Execute(main.Block);
        }

        private async Task<Either<Errors, Success>> Execute(BoundBlock block)
        {
            var statementExecutions = await block.Statements.AsyncSelect(Execute);
            
            var execResult = CombineExtensions
                .Combine(statementExecutions.TakeUntil(ExecutionFails), Errors.Concat)
                .MapRight(successes => new Success());

            return execResult;
        }

        private static bool ExecutionFails(Either<Errors, Success> either)
        {
            var executionFails = either
                .MapRight(success => false)
                .Handle(errors => errors.Any());

            return executionFails;
        }

        private async Task<Either<Errors, Success>> Execute(BoundStatement statement)
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

        private async Task<Either<Errors, Success>> Execute(BoundCallStatement boundCallStatement)
        {
            var either = await Evaluate(boundCallStatement.Call);
            return either.MapRight(o => new Success());
        }

        private async Task<Either<Errors, Success>> Execute(BoundIfStatement boundIfStatement)
        {
            var eitherComparison = await IsMet(boundIfStatement.Condition);

            var result = eitherComparison.MapRight(async isMet =>
            {
                if (isMet)
                {
                    var executeResult = await Execute(boundIfStatement.TrueBlock);

                    return executeResult;
                }
                else
                {
                    var optionalTask = boundIfStatement.FalseBlock.Map(Execute);
                    var task = (Task<Either<Errors, Success>>) optionalTask.Match(t => t, () => Task.CompletedTask);
                    return await task;
                }
            });
            
            return await result.RightTask();
        }

        private async Task<Either<Errors, bool>> IsMet(BoundCondition condition)
        {
            var left = await Evaluate(condition.Left);
            var right = await Evaluate(condition.Right);
            return CombineExtensions.Combine(left, right, (x, y) => Compare(x, y, condition.Op), Errors.Concat);
        }

        private Either<Errors, bool> Compare(object a, object b, BooleanOperator op)
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

            return new Errors(new Error(ErrorKind.TypeMismatch, $"Cannot compare '{a}' of type {a.GetType()} and '{b}' of type {b.GetType()}"));
        }

        private async Task<Either<Errors, Success>> Execute(BoundAssignmentStatement boundAssignmentStatement)
        {
            var evaluation = await Evaluate(boundAssignmentStatement.Expression);
            evaluation.WhenRight(o => variables[boundAssignmentStatement.Variable] = o);
            return evaluation.MapRight(o => new Success());
        }

        private async Task<Either<Errors, object>> Evaluate(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundIdentifier boundIdentifier:
                    return Evaluate(boundIdentifier);
                case BoundBuiltInFunctionCallExpression boundBuiltInFunctionCallExpression:
                    return await Evaluate(boundBuiltInFunctionCallExpression);
                case BoundStringExpression boundStringExpression:
                    return Evaluate(boundStringExpression);
                case BoundCustomCallExpression boundCustomCallExpression:
                    return await Evaluate(boundCustomCallExpression);
                case BoundNumericExpression boundNumericExpression:
                    return boundNumericExpression.Value;
            }

            throw new ArgumentOutOfRangeException(nameof(expression));
        }

        private Either<Errors, object> Evaluate(BoundStringExpression boundStringExpression)
        {
            return Replace(boundStringExpression.String);
        }

        private Either<Errors, object> Replace(string str)
        {
            var references = References.FromString(str);

            var refsAndValues = from variable in references
                from value in DictionaryExtensions.GetValueOrNone(variables, variable).ToEnumerable()
                select new {variable, value};

            var refsAndValuesList = refsAndValues.ToList();

            var pattern = string.Join("|", refsAndValuesList.Select(arg => $"{{{arg.variable}}}"));
            if (pattern == "")
            {
                return str;
            }

            var regex = new Regex(pattern);
            var valuesDict = refsAndValuesList.ToDictionary(x => x.variable, x => x.value);
            var result = regex.Replace(str, match =>
            {
                var skipLast = MoreLinq.MoreEnumerable.SkipLast(match.Value.Skip(1), 1);
                var key = new string(skipLast.ToArray());
                return valuesDict[key].ToString();
            });

            return result;
        }

        private async Task<Either<Errors, object>> Evaluate(BoundCustomCallExpression call)
        {
            return await Execute(call.FunctionDeclaration.Block);
        }

        private async Task<Either<Errors, object>> Evaluate(BoundBuiltInFunctionCallExpression call)
        {
            var eitherParameters = await call.Parameters.AsyncSelect(Evaluate);
            var eitherParameter = CombineExtensions.Combine(eitherParameters, Errors.Concat);

            try
            {
                var mapSuccess = eitherParameter.MapRight(async parameters => await call.Function.Invoke(parameters.ToArray()));
                var right = await mapSuccess.RightTask();
                return right;
            }
            catch (Exception ex)
            {
                return new Errors(new Error(ErrorKind.IntegratedFunctionFailure, $"Function {call.Function.Name} threw an exception: '{ex.Message}'"));
            }
        }

        private Either<Errors, object> Evaluate(BoundIdentifier identifier)
        {
            if (variables.TryGetValue(identifier.Identifier, out var val))
            {
                return val;
            }

            return new Errors(new Error(ErrorKind.VariableNotFound, $"Could not find variable '{identifier.Identifier}'"));
        }
    }
}