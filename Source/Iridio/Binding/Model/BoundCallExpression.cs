using Iridio.Core;

namespace Iridio.Binding.Model
{
    public abstract class BoundCallExpression : BoundExpression
    {
        public BoundCallExpression(Position position) : base(position)
        {
        }
    }
}