using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MoreLinq.Extensions;
using Runtime;
using SimpleScript.Binding;
using Xunit;
using Xunit.Abstractions;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Tests
{
    public class ScriptRunnerTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ScriptRunnerTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Variable_should_be_set()
        {
            var execution = await Execute("Main { a=125; }");
            execution.Variables["a"].Should().Be(125);
        }

        [Fact]
        public async Task No_main_function_produces_error()
        {
            var execution = await Execute("Function { }");
            execution.Errors.Should().ContainEquivalentOf(new Error(ErrorKind.UndefinedMainFunction), AssertConfiguration.ForErrors);
        }

        [Fact]
        public async Task Variable_set_to_function_call()
        {
            var execution = await Execute(@"Main { a = Add(1, 5); }");
            execution.Variables["a"].Should().Be(6);
        }

        [Fact]
        public async Task Conditional_block_testing_string_equality()
        {
            var input = @"Main { a=""hi""; if (a == ""hi"") { b = 1; } else { b=2; }}";
            var execution = await Execute(input);
            execution.Variables["b"].Should().Be(1);
        }

        [Fact]
        public async Task Conditional_block_testing_string_equality_else_block()
        {
            var input = @"Main { a=""hi""; if (a == ""hey"") { b = 1; } else { b=2; }}";
            var execution = await Execute(input);
            execution.Variables["b"].Should().Be(2);
        }

        [Fact]
        public async Task Call()
        {
            var execution = await Execute("Function { a=1; } Main { Function(); }");
            execution.Variables["a"].Should().Be(1);
        }

        [Fact]
        public async Task Conditional_block_testing_int_equality()
        {
            var execution = await Execute(@"Main { a=1; if (a == 1) { b = 1; } else { b=2; }}");
            execution.Variables["b"].Should().Be(1);
        }

        [Fact]
        public async Task Token_replacement()
        {
            var execution = await Execute(@"Main { name=""JMN""; greeting=""Hi {name}!""; }");
            execution.Variables["greeting"].Should().Be("Hi JMN!");
        }

        [Fact]
        public async Task Token_replacement_unset_variable()
        {
            var execution = await Execute(@"Main { greeting=""Hi {name}!""; }");
            var expectedError = new Error(ErrorKind.ReferenceToUninitializedVariable);
            execution.Errors.Should().ContainEquivalentOf(expectedError, AssertConfiguration.ForErrors);
        }

        [Theory]
        [InlineData(0, 0, "==", true)]
        [InlineData(-1, 0, "<", true)]
        [InlineData(2, 1, ">", true)]
        [InlineData(1, 1, ">=", true)]
        [InlineData(0, 1, "<=", true)]
        public async Task Int_comparison(int left, int right, string op, bool expected)
        {
            var execution = await Execute($@"Main {{ a={left}; if (a {op} {right}) {{ b = 1; }} else {{ b=2; }}}}");
            execution.Variables["b"].Should().Be(expected ? 1 : 2);
        }

        [Theory]
        [InlineData("Hi", "Hi", "==", true)]
        [InlineData("Hi", "Hey", "==", false)]
        [InlineData("Hi", "Hi", "!=", false)]
        [InlineData("Hi", "Hey", "!=", true)]
        public async Task String_comparison(string left, string right, string op, bool expected)
        {
            var execution = await Execute($@"Main {{ a=""{left}""; if (a {op} ""{right}"") {{ b = 1; }} else {{ b=2; }}}}");
            execution.Variables["b"].Should().Be(expected ? 1 : 2);
        }

        [Fact]
        public async Task TypeMismatch()
        {
            var execution = await Execute("Main { a=125; b = \"Hi\"; if (a == b) { } }");
            var expectedError = new Error(ErrorKind.TypeMismatch);
            execution.Errors.Should().ContainEquivalentOf(expectedError, AssertConfiguration.ForErrors);
        }

        private async Task<ExecutionSummary> Execute(string input)
        {
            var variables = new Dictionary<string, object>();
            var sut = CreateSut();
            var result = await sut.Run(input, variables);

            var executionSummary = result
                .MapRight(s => new ExecutionSummary(true, variables, new Errors()))
                .Handle(errors => new ExecutionSummary(false, variables, errors));

            executionSummary.Errors.ForEach(s => testOutputHelper.WriteLine(s.ToString()));

            return executionSummary;
        }

        private IScriptRunner CreateSut()
        {
            var functions = new IFunction[] {new Function("Func1"), new LambdaFunction<int, int, int>("Add", (a, b) => a + b),  };
            return new ScriptRunner(functions, new EnhancedParser(), new Binder(new BindingContext(functions)));
        }
    }
}