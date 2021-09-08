using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class CallStatement : Statement
    {
        public CallExpression Call { get; }

        public CallStatement(CallExpression call, Position position) : base(position)
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