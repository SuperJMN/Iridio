using System.Collections.Generic;
using System.Linq;
using Iridio.Preprocessing;

namespace Iridio.Tests.Execution
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
            return new InMemoryTextFile(dictionary[path]);
        }

        private readonly ICollection<string> workingDirsHistory = new List<string> {""};

        public string WorkingDirectory
        {
            get => workingDirsHistory.LastOrDefault();
            set => workingDirsHistory.Add(value);
        }
    }
}