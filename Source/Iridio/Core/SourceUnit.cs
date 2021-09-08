using System;
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
            if (column < 0)
            {
                throw new InvalidOperationException($"Column cannot be negative: {column}");
            }

            var contentLength = line.Content.Length;
            if (column > contentLength)
            {
                throw new InvalidOperationException(
                    $"Column cannot be higher than the number of chars in the line {column} of a maximum of {contentLength} (content: {line.Content})");
            }

            Line = line;
            Column = column;
        }

        public static SourceUnit From(Position absolutePosition, SourceCode sourceCode)
        {
            var lineNumber = absolutePosition.Line - 1;

            if (lineNumber < 0)
            {
                throw new InvalidOperationException("Cannot get a negative line");
            }

            if (lineNumber > sourceCode.Lines.Count)
            {
                throw new InvalidOperationException($"Cannot get a line {lineNumber} from source, that has {sourceCode.Lines.Count} lines in total");
            }

            var line = sourceCode.Lines[lineNumber];

            return new SourceUnit(line, absolutePosition.Column);
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