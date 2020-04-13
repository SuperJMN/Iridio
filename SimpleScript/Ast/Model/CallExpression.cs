using System.Linq;

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

        public override string ToString()
        {
            return $"{FuncName}({string.Join(",", Parameters.Select(x => x.ToString()))})";
        }
    }
}