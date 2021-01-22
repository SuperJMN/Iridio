using System.Globalization;

namespace Iridio.Parsing.Model
{
    public class DoubleExpression : Expression
    {
        public DoubleExpression(in double n)
        {
            Value = n;
        }

        public double Value { get; }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}