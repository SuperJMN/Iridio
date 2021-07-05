using System.Collections.Generic;
using System.Linq;
using Iridio.Preprocessing;

namespace Iridio.Parsing
{
    public class CompilerInput
    {
        private readonly IEnumerable<TaggedLine> taggedLines;

        public CompilerInput(IEnumerable<TaggedLine> taggedLines)
        {
            this.taggedLines = taggedLines;
        }

        public string Join()
        {
            return string.Join("\n", taggedLines.Select(x => x.Content));
        }
    }
}