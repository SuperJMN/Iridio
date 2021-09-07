using CSharpFunctionalExtensions;
using Iridio.Core;

namespace Iridio.Runtime
{
    public abstract class RunError
    {
        public Maybe<Position> Position { get; }

        protected RunError(Maybe<Position> position)
        {
            Position = position;
        }
    }
}