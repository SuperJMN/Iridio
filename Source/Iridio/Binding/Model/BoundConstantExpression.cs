using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundConstantExpression : BoundExpression
    {
        public object Value { get; }

        public BoundConstantExpression(object value, Position position) : base(position)
        {
            Value = value;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            if (Value is string)
            {
                return "\"" + Value + "\"";
            }

            return Value.ToString();
        }
    }
}