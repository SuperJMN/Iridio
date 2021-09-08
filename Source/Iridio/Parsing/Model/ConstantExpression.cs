using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class ConstantExpression : Expression
    {
        public object Value { get; }

        public ConstantExpression(object value, Position position) : base(position)
        {
            Value = value;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}