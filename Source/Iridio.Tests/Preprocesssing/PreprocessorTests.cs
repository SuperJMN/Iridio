using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Iridio.Preprocessing;
using Iridio.Tests.TestDoubles;
using Xunit;

namespace Iridio.Tests.Preprocesssing
{
    public class PreprocessorTests
    {
        [Theory]
        [InlineData("main.rdo", new[] {"main.rdo:Hi"}, "Hi")]
        [InlineData("main.rdo", new[] {"main.rdo:#include other.txt\nMario", "other.txt:Hi"}, "Hi\nMario")]
        [InlineData("main.rdo", new[] {"main.rdo:// Comment\nHi all!"}, "Hi all!")]
        [InlineData("root\\subdir\\main.rdo", new[] {"root\\subdir\\main.rdo:#include ..\\other.rdo", "root\\other.rdo:Hi"}, "Hi")]
        public void Include(string mainScript, string[] files, string expected)
        {
            var sut = CreateSut(files);

            var result = sut.Process(mainScript);
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

        [Fact(Skip = "Uses local file")]
        public void Integration()
        {
            var sut = new Preprocessor(new FileSystem());
            var result = sut.Process(@"C:\Users\JMN\Extended\Fast\Repos\WOA-Project\Deployment-Feed\Devices\Lumia\950s\Cityman\Main.txt");
        }
    }
}