using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class AssignmentStatement : Statement
    {
        public string Target { get; }
        public Expression Expression { get; }

        public AssignmentStatement(string target, Position position, Expression expression) : base(position)
        {
            Target = target;
            Expression = expression;
        }

        public override string ToString()
        {
            return $"{Target}={Expression}";
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}