using Iridio.Binding.Model;

namespace Iridio.Binding
{
    public class BoundBooleanValueExpression : BoundExpression
    {
        public bool Value { get; }

        public BoundBooleanValueExpression(bool value)
        {
            Value = value;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}