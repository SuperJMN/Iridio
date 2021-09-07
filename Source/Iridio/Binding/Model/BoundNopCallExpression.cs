using Iridio.Core;

namespace Iridio.Binding.Model
{
    internal class BoundNopCallExpression : BoundCallExpression
    {
        public override void Accept(IBoundNodeVisitor visitor)
        {
        }

        public BoundNopCallExpression(Position position) : base(position)
        {
        }
    }
}