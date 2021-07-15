using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Core;
using Iridio.Parsing;
using Iridio.Preprocessing;
using Moq;
using Xunit;

namespace Iridio.Tests.Compilation
{
    public class CompilerTests
    {
        [Fact]
        public void Test()
        {
            var mock = new Mock<IParser>();
            mock.Setup(x => x.Parse(It.IsAny<string>()))
                .Returns(() => new ParsingError(new Position(2, 1), "El oso no se puede compilar"));
            var sut = new SourceCodeCompiler(Mock.Of<IBinder>(), mock.Object);
            var source = SourceCode.FromString("Un té de tilo, para el cocodrilo\nUn remedio hermoso, para darle al oso");
            var result = sut.Compile(source);

            result.Should()
                .BeFailure()
                .And
                .Subject.Error.Should().BeOfType<ParseError>()
                .Which.SourceUnit.Should().Be(SourceUnit.From(new Position(2, 1), source));
        }

        [Fact]
        public void Syntax_error_has_correct_position_information()
        {
            var sut = CreateSut();

            var sourceCode = new SourceCode(new List<Line>
            {
                new Line("Main {", 1, "file.rdo"),
                new Line("a = 10;", 1, "child.rdo"),
                new Line("FAIL ME BIG TIME!;", 2, "child.rdo")
            });

            var result = sut.Compile(sourceCode);

            var sourceUnit = SourceUnit.From(new Position(3, 1), sourceCode);

            result.Should().BeFailure()
                .And
                .Subject.Error.Should().BeOfType<ParseError>()
                .Which.SourceUnit.Should()
                .BeEquivalentTo(sourceUnit);
        }

        private static SourceCodeCompiler CreateSut()
        {
            var functionDeclarations = Enumerable.Empty<IFunctionDeclaration>();
            var sut = new SourceCodeCompiler(new Binder(functionDeclarations), new Parser());
            return sut;
        }
    }
}