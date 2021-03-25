namespace Iridio.Parsing.Model
{
    public class BooleanValueExpression : Expression
    {
        public BooleanValueExpression(bool value)
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