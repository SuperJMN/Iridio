using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SimpleScript.Binding;
using Xunit;
using Zafiro.Core.Patterns;

namespace SimpleScript.Tests
{
    public class ScriptRunnerTests
    {
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
            execution.Errors.Should().Contain("Main function not defined");
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

        private async Task<ExecutionSummary> Execute(string input)
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
            var functions = new IFunction[] {new Function("Func1"), new LambdaFunction<int, int, int>("Add", (a, b) => a + b),  };
            return new ScriptRunner(functions, new EnhancedParser(), new Binder(new BindingContext(functions)));
        }
    }
}