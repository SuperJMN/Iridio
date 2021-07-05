using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Iridio.Preprocessing;
using Xunit;
using Zafiro.Core.Mixins;

namespace Iridio.Tests
{
    public class PreprocessorTests
    {
        [Theory]
        [InlineData(new[] {"main.rdo:Hi"}, "Hi")]
        [InlineData(new[] {"main.rdo:#include other.txt\nMario", "other.txt:Hi"}, "Hi\nMario")]
        public void Include(string[] files, string expected)
        {
            var sut = CreateSut(files);

            var result = sut.Process("main.rdo");
            result.Join().Should().Be(expected);
        }

        private static Dictionary<string, string> BuildFileSystemDictionary(string[] files)
        {
            return files.Select(s =>
            {
                var strings = s.Split(":");
                return (strings[0], strings[1]);
            }).ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        private IPreprocessor CreateSut(string[] filesystem)
        {
            var directoryContext = new TestDirectoryContext();
            ITextFileFactory testFileFactory = new TestFileFactory(BuildFileSystemDictionary(filesystem));
            return new Preprocessor(testFileFactory, directoryContext);
        }

        private class TestDirectoryContext : IDirectoryContext
        {
            private readonly ICollection<string> workingDirsHistory = new List<string> {""};

            public string WorkingDirectory
            {
                get => workingDirsHistory.LastOrDefault();
                set => workingDirsHistory.Add(value);
            }
        }

        private class TestFileFactory : ITextFileFactory
        {
            private readonly Dictionary<string, string> dictionary;

            public TestFileFactory(Dictionary<string, string> dictionary)
            {
                this.dictionary = dictionary;
            }

            public ITextFile Get(string path)
            {
                return new TestTextFile(dictionary[path]);
            }
        }

        private class TestTextFile : ITextFile
        {
            private readonly string contents;

            public TestTextFile(string contents)
            {
                this.contents = contents;
            }

            public IEnumerable<string> Lines()
            {
                return contents.Lines();
            }
        }
    }
}