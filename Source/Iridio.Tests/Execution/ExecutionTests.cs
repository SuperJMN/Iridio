using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Iridio.Common;
using Iridio.Runtime;
using Moq;
using Xunit;
using Zafiro.Core.FileSystem;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Tests.Execution
{
    public class ExecutionTests
    {
        [Fact]
        public async Task Constant_assignment()
        {
            var vars = await Run("Main{ a = 13; }");
            vars
                .MapRight(x => x.Variables["a"])
                .Should().Be(Either.Success<string, object>(13));
        }

        [Fact]
        public async Task Addition_assignment()
        {
            var vars = await Run("Main{ a = 1+3; }");
            vars
                .MapRight(x => x.Variables["a"])
                .Should().Be(Either.Success<string, object>(4));
        }

        private static async Task<Either<string, Runtime.ReturnValues.ExecutionSummary>> Run(string source)
        {
            var fsoMock = new Mock<IFileSystemOperations>();
            fsoMock.Setup(fso => fso.ReadAllText(It.IsAny<string>())).Returns(source);
            fsoMock.SetupGet(x => x.WorkingDirectory).Returns("");
            var compiler = new Compiler(fsoMock.Object);

            return await compiler.Compile("source")
                .MapLeft(x => x.ToString())
                .MapRight(async script =>
                {
                    var runner = new ScriptRunner(Enumerable.Empty<IFunction>());
                    var runResult = await runner.Run(script);

                    return runResult
                        .MapLeft(x => x.ToString());
                }).RightTask();
        }
    }
}