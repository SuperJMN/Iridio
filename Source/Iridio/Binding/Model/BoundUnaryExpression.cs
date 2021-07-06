using Iridio.Parsing.Model;

namespace Iridio.Binding.Model
{
    public class BoundUnaryExpression : BoundExpression
    {
        public BoundExpression Expression { get; }
        public UnaryOperator Op { get; }

        public BoundUnaryExpression(BoundExpression expression, UnaryOperator op)
        {
            Expression = expression;
            Op = op;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}