namespace SimpleScript.Parsing.Model
{
    internal class Condition
    {
        public Expression Left { get; }
        public BooleanOperator Op { get; }
        public Expression Right { get; }

        public Condition(Expression left, BooleanOperator op, Expression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    }
}