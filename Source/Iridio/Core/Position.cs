using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Iridio.Core
{
    public class Position : ValueObject
    {
        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public int Line { get; }
        public int Column { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Line;
            yield return Column;
        }
    }
}