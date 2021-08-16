using Iridio.Core;

namespace Iridio.Runtime
{
    public abstract class RunError
    {
        public Position Position { get; }

        protected RunError(Position position)
        {
            Position = position;
        }
    }
}