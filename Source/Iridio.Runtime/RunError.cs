using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Runtime
{
    public abstract class RunError : IErrors
    {
        public Position Position { get; }

        protected RunError(Position position)
        {
            Position = position;
        }

        public abstract IReadOnlyCollection<Error> Errors { get; }
    }
}