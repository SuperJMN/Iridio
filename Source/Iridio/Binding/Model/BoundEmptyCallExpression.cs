using Iridio.Core;

namespace Iridio.Binding.Model
{
    internal class BoundEmptyCallExpression : BoundCallExpression
    {
        public override void Accept(IBoundNodeVisitor visitor)
        {
        }

        public BoundEmptyCallExpression(Position position) : base(position)
        {
        }
    }
}