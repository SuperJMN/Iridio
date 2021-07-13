using System.Collections.Generic;
using Iridio.Preprocessing;
using Zafiro.Core.Mixins;

namespace Iridio.Tests
{
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
}