using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Iridio.Common;
using Iridio.Runtime;
using Iridio.Runtime.ReturnValues;
using Moq;
using Optional;
using Xunit;
using Zafiro.Core;
using Zafiro.Core.FileSystem;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Tests.Execution
{
    public class ExecutionTests
    {
        [Fact]
        public async Task Add()
        {
            var source = "Main{ a = 13; }";
            var fsoMock = new Mock<IFileSystemOperations>();
            fsoMock.Setup(fso => fso.ReadAllText(It.IsAny<string>())).Returns(source);
            fsoMock.SetupGet(x => x.WorkingDirectory).Returns("");
            var compiler = new Compiler(fsoMock.Object);
            var compiled = await compiler.Compile("source")
                .MapRight(async script =>
                {
                    var runner = new ScriptRunner(Enumerable.Empty<IFunction>());
                    var either = await runner.Run(script);
                    return either.MapLeft(x => new Errors());
                }).RightTask();

            compiled.Should().BeEquivalentTo(Either.Success<RuntimeErrors, Success>(new Success()));
        }

        [Fact]
        public void EitherTest()
        {
            var a = Either.Success<string, string>("");
            var b = Either.Success<string, string>("");
            a.Should().BeEquivalentTo(b);
        }

        [Fact]
        public void EitherTest2()
        {
            var a = Either.Success<string, Success>(new Success());
            var b = Either.Success<string, Success>(new Success());
            a.Should().BeEquivalentTo(b, options => options
                .ComparingByMembers(typeof(Either<,>))
                .ComparingByMembers(typeof(Option<>))
            //    .Using(new LambdaComparer<Success>((x, y) => true))
            );
        }
    }
}