using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundBooleanValueExpression : BoundExpression
    {
        public bool Value { get; }

        public BoundBooleanValueExpression(bool value, Position position) : base(position)
        {
            Value = value;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}