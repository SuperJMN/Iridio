using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Preprocessing;
using Zafiro.Core.Mixins;

namespace Iridio
{
    public class SourceCode
    {
        public IList<Line> Lines { get; }

        public SourceCode(IList<Line> lines)
        {
            Lines = lines;
        }

        public string Text
        {
            get { return string.Join("\n", Lines.Select(line => line.IsComment ? "" : line.Content)); }
        }

        public static SourceCode FromString(string source)
        {
            var lines = source.Lines().Select((s, i) => new Line(s, i + 1, Maybe<string>.None));
            return new SourceCode(lines.ToList());
        }
    }
}