namespace SimpleScript.Ast.Model
{
    internal class AssignmentStatement : Statement
    {
        public string Variable { get; }
        public Expression Expression { get; }

        public AssignmentStatement(string variable, Expression expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }
}