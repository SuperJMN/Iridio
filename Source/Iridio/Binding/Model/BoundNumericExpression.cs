namespace Iridio.Binding.Model
{
    public class BoundNumericExpression : BoundExpression
    {
        public int Value { get; }

        public BoundNumericExpression(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}