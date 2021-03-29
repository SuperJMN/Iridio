using Iridio.Parsing.Model;

namespace Iridio.Binding.Model
{
    public class BoundCondition : BoundExpression
    {
        public BoundExpression Left { get; }
        public BinaryOperator Op { get; }
        public BoundExpression Right { get; }

        public BoundCondition(BoundExpression left, BinaryOperator op, BoundExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}