using Iridio.Tokenization;
using Superpower.Model;

namespace Iridio.Parsing.Model
{
    public class AssignmentStatement : Statement
    {
        public string Variable { get; }
        public Expression Expression { get; }

        public AssignmentStatement(Token<SimpleToken> variable, Expression expression)
        {
            Variable = variable.ToStringValue();
            Expression = expression;
        }

        public override string ToString()
        {
            return $"{Variable}={Expression}";
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}