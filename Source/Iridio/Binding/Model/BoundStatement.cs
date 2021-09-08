using CSharpFunctionalExtensions;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public abstract class BoundStatement : IBoundNode
    {
        protected BoundStatement(Maybe<Position> position)
        {
            Position = position;
        }

        public abstract void Accept(IBoundNodeVisitor visitor);
        public Maybe<Position> Position { get; }
    }
}