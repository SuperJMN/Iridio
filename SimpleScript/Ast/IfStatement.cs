using SimpleScript.Ast.Model;

namespace SimpleScript.Ast
{
    internal class IfStatement : Statement
    {
        public Condition Cond { get; }
        public Statement[] IfStatements { get; }
        public Statement[] ElseStatements { get; }

        public IfStatement(Condition cond, Statement[] ifStatements, Statement[] elseStatements)
        {
            Cond = cond;
            IfStatements = ifStatements;
            ElseStatements = elseStatements;
        }
    }
}