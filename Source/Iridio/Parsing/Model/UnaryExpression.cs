namespace Iridio.Parsing.Model
{
    public class UnaryExpression : Expression
    {
        public UnaryOperator Op { get; }
        public Expression Expression { get; }

        public UnaryExpression(UnaryOperator op, Expression expression)
        {
            Op = op;
            Expression = expression;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}