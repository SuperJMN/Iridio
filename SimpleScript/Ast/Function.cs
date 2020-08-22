using SimpleScript.Ast.Model;

namespace SimpleScript.Ast
{
    public class Function
    {
        public string Name { get; }
        public Statement[] Statements { get; }

        public Function(string name, Statement[] statements)
        {
            Name = name;
            Statements = statements;
        }
    }
}