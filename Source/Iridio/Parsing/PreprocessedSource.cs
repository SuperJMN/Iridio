using System.Collections.Generic;
using System.Linq;
using Iridio.Preprocessing;

namespace Iridio.Parsing
{
    public class PreprocessedSource
    {
        public IList<TaggedLine> TaggedLines { get; }

        public PreprocessedSource(IList<TaggedLine> taggedLines)
        {
            TaggedLines = taggedLines;
        }

        public string Text
        {
            get { return string.Join("\n", TaggedLines.Where(t => !t.IsComment).Select(x => x.Content)); }
        }
    }
}