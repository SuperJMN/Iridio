namespace SimpleScript.Binding
{
    internal class BoundNumericExpression : BoundExpression
    {
        public int Value { get; }

        public BoundNumericExpression(int value)
        {
            Value = value;
        }
    }
}