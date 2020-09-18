namespace Iridio.Parsing.Model
{
    public class AssignmentStatement : Statement
    {
        public string Variable { get; }
        public Expression Expression { get; }

        public AssignmentStatement(string variable, Expression expression)
        {
            Variable = variable;
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