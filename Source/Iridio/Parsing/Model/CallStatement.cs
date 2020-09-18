namespace Iridio.Parsing.Model
{
    public class CallStatement : Statement
    {
        public CallExpression Call { get; }

        public CallStatement(CallExpression call)
        {
            Call = call;
        }

        public override string ToString()
        {
            return $"Call: {Call}";
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}