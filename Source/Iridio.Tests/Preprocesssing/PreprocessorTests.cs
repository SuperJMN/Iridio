using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using FluentAssertions;
using Iridio.Preprocessing;
using Xunit;

namespace Iridio.Tests.Preprocesssing
{
    public class PreprocessorTests
    {
        [Theory]
        [InlineData("main.rdo", new[] {"main.rdo:Hi"}, "Hi")]
        [InlineData("main.rdo", new[] {"main.rdo:#include other.txt\nMario", "other.txt:Hi"}, "Hi\nMario")]
        [InlineData("main.rdo", new[] {"main.rdo:// Comment\nHi all!"}, "Hi all!")]
        public void Include(string mainScript, string[] files, string expected)
        {
            var sut = CreateSut(files);

            var result = sut.Process(mainScript);
            result.Text.Should().Be(expected);
        }

        private static IDictionary<string, MockFileData> BuildFileSystemDictionary(IEnumerable<string> files)
        {
            return files.Select(s =>
            {
                var strings = s.Split(":");
                return (strings[0], strings[1]);
            }).ToDictionary(tuple => tuple.Item1, tuple => new MockFileData(tuple.Item2));
        }

        private IPreprocessor CreateSut(string[] filesystem)
        {
            return new Preprocessor(new MockFileSystem(BuildFileSystemDictionary(filesystem)));
        }
    }
}