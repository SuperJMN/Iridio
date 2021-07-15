using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Core;
using Iridio.Parsing;
using Moq;
using Xunit;

namespace Iridio.Tests.Compilation
{
    public class UnitTest1
    {
        [Fact]
        public void Test()
        {
            var mock = new Mock<IParser>();
            mock.Setup(x => x.Parse(It.IsAny<string>()))
                .Returns(() => new ParsingError(new Position(2, 1), "El oso no se puede compilar"));
            var sut = new SourceCodeSourceCodeCompiler(Mock.Of<IBinder>(), mock.Object);
            var result = sut.Compile(SourceCode.FromString("Un té de tilo, para el cocodrilo\nUn remedio hermoso, para darle al oso"));

            result.Should()
                .BeFailure()
                .And
                .Subject.Error.Should().BeOfType<ParseError>()
                .Which.Location.Line.Should().Be(2);

            //.Which.Location.
        }
    }
}
