using FluentAssertions;
using Moq;
using Xunit;
using Zafiro.Core.FileSystem;

namespace Iridio.Tests
{
    public class PreprocessorTests
    {
        [Theory]
        [InlineData("#include File.ss\nThis is some code;", "This is more code;\r\nThis is some code;")]
        [InlineData("//this is a comment", "")]
        public void Verify(string input, string expected)
        {
            var sut = CreatePreprocessor();

            var result = sut.Process(input);

            result.Should().Be(expected);
        }

        private IPreprocessor CreatePreprocessor()
        {
            var mock = new Mock<IFileSystemOperations>();
            mock.Setup(f => f.ReadAllText(It.Is<string>(s => s == "File.ss"))).Returns("This is more code;");
            return new Preprocessor(mock.Object);
        }
    }
}