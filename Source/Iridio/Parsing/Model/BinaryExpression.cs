namespace Iridio.Parsing.Model
{
    public class BinaryExpression : Expression
    {
        public Operator Op { get; }
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryExpression(Operator op, Expression left, Expression right)
        {
            Op = op;
            Left = left;
            Right = right;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}