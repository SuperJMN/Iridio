namespace SimpleScript.Ast.Model
{
    public class CallExpression : Expression
    {
        public string FuncName { get; }
        public Expression[] Parameters { get; }

        public CallExpression(string funcName, Expression[] parameters)
        {
            FuncName = funcName;
            Parameters = parameters;
        }
    }
}