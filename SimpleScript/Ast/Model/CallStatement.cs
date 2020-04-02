namespace SimpleScript.Ast.Model
{
    public class CallStatement : Statement
    {
        public Expression Expression { get; }

        public CallStatement(Expression expression)
        {
            Expression = expression;
        }
    }
}