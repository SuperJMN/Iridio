using System.Collections.Generic;
using System.Linq;
using Iridio.Preprocessing;
using Zafiro.Core.Mixins;

namespace Iridio.Parsing
{
    public class SourceCode
    {
        public IList<Line> TaggedLines { get; }

        public SourceCode(IList<Line> taggedLines)
        {
            TaggedLines = taggedLines;
        }

        public string Text
        {
            get { return string.Join("\n", TaggedLines.Where(t => !t.IsComment).Select(x => x.Content)); }
        }

        public static IList<Line> FromString(string source)
        {
            return source.Lines().Select((s, i) => new Line(s, "<not-applicable>", i + 1)).ToList();
        }
    }
}