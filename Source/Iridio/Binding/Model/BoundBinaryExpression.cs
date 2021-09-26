using Iridio.Core;
using Iridio.Parsing.Model;

namespace Iridio.Binding.Model
{
    public class BoundBinaryExpression : BoundExpression
    {
        public BoundExpression Left { get; }
        public BinaryOperator Op { get; }
        public BoundExpression Right { get; }

        public BoundBinaryExpression(BoundExpression left, BinaryOperator op, BoundExpression right, Position position) : base(position)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{Left} {Op} {Right}";
        }
    }
}