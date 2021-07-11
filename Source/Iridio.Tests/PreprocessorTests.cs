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
            var directoryContext = new InMemoryDirectoryContext();
            ITextFileFactory testFileFactory = new DictionaryBasedTextFileFactory(BuildFileSystemDictionary(filesystem));
            return new Preprocessor(testFileFactory, directoryContext);
        }
    }

    internal class DictionaryBasedTextFileFactory : ITextFileFactory
    {
        private readonly Dictionary<string, string> dictionary;

        public DictionaryBasedTextFileFactory(Dictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
        }

        public ITextFile Get(string path)
        {
            return new InMemoryTextFile(dictionary[path]);
        }
    }

    internal class InMemoryTextFile : ITextFile
    {
        private readonly string contents;

        public InMemoryTextFile(string contents)
        {
            this.contents = contents;
        }

        public IEnumerable<string> Lines()
        {
            return contents.Lines();
        }
    }

    internal class InMemoryDirectoryContext : IDirectoryContext
    {
        private readonly ICollection<string> workingDirsHistory = new List<string> {""};

        public string WorkingDirectory
        {
            get => workingDirsHistory.LastOrDefault();
            set => workingDirsHistory.Add(value);
        }
    }
}