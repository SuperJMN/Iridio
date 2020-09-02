using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Optional.Unsafe;
using SimpleScript.Binding;
using SimpleScript.Binding.Model;
using Xunit;
using Zafiro.Core.Patterns;

namespace SimpleScript.Tests
{
    public class ScriptRunnerTests
    {
        [Fact]
        public async Task Variable_should_be_set()
        {
            var execution = await VerifyExecution("Main { a=125; }");
            execution.Variables["a"].Should().Be(125);
        }

        [Fact]
        public async Task No_main_function_produces_error()
        {
            var execution = await VerifyExecution("Function { }");
            execution.Errors.Should().Contain("Main function not defined");
        }

        [Fact]
        public async Task Variable_set_to_function_call()
        {
            var execution = await VerifyExecution(@"Main { a = Suma(1, 5); }");
            execution.Variables["a"].Should().Be(6);
        }

        private async Task<ExecutionSummary> VerifyExecution(string input)
        {
            var variables = new Dictionary<string, object>();
            var sut = CreateSut();
            var result = await sut.Run(input, variables);

            return result
                .MapSuccess(s => new ExecutionSummary(true, variables, new ErrorList(new List<string>())))
                .Handle(errors => new ExecutionSummary(false, variables, errors)); ;
        }

        private IScriptRunner CreateSut()
        {
            var functions = new IFunction[] {new Function("Func1"), new LambdaFunction<int, int, int>("Suma", (a, b) => a + b),  };
            return new ScriptRunner(functions, new EnhancedParser(), new Binder(new BindingContext(functions)));
        }
    }

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

            if (mapSuccess.IsRight)
            {
                return await mapSuccess.RightValue.ValueOrFailure();
            }
            else
            {
                return mapSuccess.LeftValue.ValueOrFailure();
            }
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
            var asyncSelect = await block.BoundStatements.AsyncSelect(statement => Execute(statement));

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
                    await Execute(boundAssignmentStatement);
                    break;
                case BoundCallStatement boundCallStatement:
                    break;
                case BoundCondition boundCondition:
                    break;
                case BoundEchoStatement boundEchoStatement:
                    break;
                case BoundIfStatement boundIfStatement:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement));
            }

            return new Success();
        }

        private async Task Execute(BoundAssignmentStatement boundAssignmentStatement)
        {
            variables[boundAssignmentStatement.Variable] = await Evaluate(boundAssignmentStatement.Expression);
        }

        private async Task<object> Evaluate(BoundExpression expression)
        {
            switch (expression)
            {
                case BoundIdentifier boundIdentifier:
                    break;
                case BoundBuiltInFunctionCallExpression boundBuiltInFunctionCallExpression:
                    var parameters = await boundBuiltInFunctionCallExpression.Parameters.AsyncSelect(Evaluate);
                    var invoke = await boundBuiltInFunctionCallExpression.Function.Invoke(parameters.ToArray());
                    return invoke;
                case BoundStringExpression boundStringExpression:
                    return boundStringExpression.String;
                case BoundCustomCallExpression boundCustomCallExpression:

                    break;
                
                case BoundNumericExpression boundNumericExpression:
                    return boundNumericExpression.Value;
            }

            throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }
}