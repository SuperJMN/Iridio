using System.Collections.Generic;
using System.Linq;
using Iridio.Preprocessing;

namespace Iridio.Parsing
{
    public class CompilerInput
    {
        public IList<TaggedLine> TaggedLines { get; }

        public CompilerInput(IList<TaggedLine> taggedLines)
        {
            TaggedLines = taggedLines;
        }

        public string Stringify()
        {
            return string.Join("\n", TaggedLines.Select(x => x.Content));
        }
    }
}