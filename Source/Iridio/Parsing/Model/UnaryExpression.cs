namespace Iridio.Parsing.Model
{
    public class UnaryExpression : Expression
    {
        public Operator Op { get; }
        public Expression Expression { get; }

        public UnaryExpression(Operator op, Expression expression)
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