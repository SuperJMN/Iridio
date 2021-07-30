using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iridio.Preprocessing;

namespace Iridio.Tests.TestDoubles
{
    internal class TestFileSystem : IFileSystem
    {
        private readonly Dictionary<string, string> dictionary;

        public TestFileSystem(Dictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
        }

        public ITextFile Get(string path)
        {
            var fullPath = Path.Combine(WorkingDirectory, path);
            var contents = dictionary[fullPath];
            return new InMemoryTextFile(contents);
        }

        private readonly ICollection<string> workingDirsHistory = new List<string> {""};

        public string WorkingDirectory
        {
            get => workingDirsHistory.LastOrDefault();
            set => workingDirsHistory.Add(value);
        }
    }
}