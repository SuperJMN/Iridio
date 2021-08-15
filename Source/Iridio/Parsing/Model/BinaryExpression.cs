using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class BinaryExpression : Expression
    {
        public BinaryOperator Op { get; }
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryExpression(BinaryOperator op, Expression left, Expression right, Position position) : base(position)
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