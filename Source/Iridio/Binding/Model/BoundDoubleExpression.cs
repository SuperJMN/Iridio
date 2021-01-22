using System.Globalization;

namespace Iridio.Binding.Model
{
    public class BoundDoubleExpression : BoundExpression
    {
        public double Value { get; }

        public BoundDoubleExpression(double value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}