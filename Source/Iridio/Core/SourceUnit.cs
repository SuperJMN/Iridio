using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Iridio.Parsing;
using Iridio.Preprocessing;

namespace Iridio.Core
{
    public class SourceUnit : ValueObject
    {
        public int Column { get; }
        public Line Line { get; }

        private SourceUnit(Line line, int column)
        {
            Line = line;
            Column = column;
        }

        public static SourceUnit From(Position absolutePosition, SourceCode sourceCode)
        {
            if (absolutePosition == new Position(0, 0))
            {
                absolutePosition = new Position(1, 0);
            }

            var line = sourceCode.Lines[absolutePosition.Line - 1];
            var location = new SourceUnit(line, absolutePosition.Column);
            return location;
        }

        public override string ToString()
        {
            return $"[{Line.Number}, {Column}]: {Line.Content} {Line.Path.Unwrap(s => $" - at {s}")}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Column;
            yield return Line;
        }
    }
}