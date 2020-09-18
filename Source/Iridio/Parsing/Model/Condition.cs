namespace Iridio.Parsing.Model
{
    public class Condition : ISyntax
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

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}