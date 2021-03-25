using Iridio.Binding.Model;
using Iridio.Parsing.Model;

namespace Iridio.Binding
{
    public class BoundBinaryExpression : BoundExpression
    {
        public BoundExpression Left { get; }
        public Operator Op { get; }
        public BoundExpression Right { get; }

        public BoundBinaryExpression(BoundExpression left, Operator op, BoundExpression right)
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