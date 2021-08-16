using Iridio.Core;

namespace Iridio.Binding.Model
{
    public abstract class BoundExpression : IBoundNode
    {
        public BoundExpression(Position position)
        {
            Position = position;
        }

        public abstract void Accept(IBoundNodeVisitor visitor);
        public Position Position { get; }
    }
}