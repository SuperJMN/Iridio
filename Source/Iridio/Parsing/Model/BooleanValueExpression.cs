using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class BooleanValueExpression : Expression
    {
        public BooleanValueExpression(bool value, Position position) : base(position)
        {
            Value = value;
        }

        public bool Value { get; }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}