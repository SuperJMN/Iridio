using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing;
using Iridio.Runtime;
using Xunit;

namespace Iridio.Tests.Execution
{
    public class ExecutionTests
    {
        [Theory]
        [InlineData("a = 5;", 5)]
        [InlineData("a = 5d;", 5d)]
        [InlineData("a = 1+2;", 3)]
        [InlineData("a = 5-7;", -2)]
        [InlineData("a = 3*2;", 6)]
        [InlineData("a = 6/2;", 3)]
        [InlineData("a = 5d/2;", 2.5D)]
        [InlineData("a = 12/2*3;", 18)]
        [InlineData("a = 12/(2*3);", 2)]
        [InlineData("a = 12==4;", false)]
        [InlineData("a = 12!=4;", true)]
        [InlineData("a = 4==4;", true)]
        [InlineData("a = true;", true)]
        [InlineData("a = true || false;", true)]
        [InlineData("a = true && false;", false)]
        [InlineData("a = !true;", false)]
        [InlineData("a = !false;", true)]
        [InlineData("a=0; b = 1; if (b == 1)  { a = 2; }  else  { a = 6; }", 2)]
        [InlineData("a=0; b = 5; if (b == 1) { a = 2; } else { a = 6; }", 6)]
        [InlineData("b=1; c=2; a = Add(b, c);", 3)]
        [InlineData("b=\"Hello\"; a = \"{b} world!\";", "Hello world!")]
        public async Task SimpleAssignment(string source, object expected)
        {
            var vars = await Run(Main(source));
            vars
                .Map(x => x.Variables["a"])
                .Should().BeSuccess()
                .And
                .Subject.Value.Should().Be(expected);
        }


        private static string Main(string content)
        {
            return $"Main {{ {content} }}";
        }

        private static async Task<Result<ExecutionSummary, RuntimeError>> Run(string source)
        {
            var functions = new List<IFunction>
            {
                new LambdaFunction<int, int, int>("Add", (a, b) => a + b)
            };

            var compiler = new SourceCodeCompiler(new Binder(functions), new Parser());
            var runtime = new IridioRuntime(compiler, new ScriptRunner(functions));
            return await runtime.Run(SourceCode.FromString(source));
        }
    }
}