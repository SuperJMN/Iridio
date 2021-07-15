using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Iridio.Preprocessing;
using Iridio.Tests.Execution;
using Xunit;

namespace Iridio.Tests
{
    public class PreprocessorTests
    {
        [Theory]
        [InlineData(new[] {"main.rdo:Hi"}, "Hi")]
        [InlineData(new[] {"main.rdo:#include other.txt\nMario", "other.txt:Hi"}, "Hi\nMario")]
        [InlineData(new[] {"main.rdo:// Comment\nHi all!"}, "Hi all!")]
        public void Include(string[] files, string expected)
        {
            var sut = CreateSut(files);

            var result = sut.Process("main.rdo");
            result.Text.Should().Be(expected);
        }

        private static Dictionary<string, string> BuildFileSystemDictionary(IEnumerable<string> files)
        {
            return files.Select(s =>
            {
                var strings = s.Split(":");
                return (strings[0], strings[1]);
            }).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        private IPreprocessor CreateSut(string[] filesystem)
        {
            return new Preprocessor(new TestFileSystem(BuildFileSystemDictionary(filesystem)));
        }
    }
}