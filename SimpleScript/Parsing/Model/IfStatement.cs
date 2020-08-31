using System;

namespace SimpleScript.Parsing.Model
{
    internal class IfStatement : Statement
    {
        public Condition Cond { get; }
        public Statement[] IfStatements { get; }
        public Statement[] ElseStatements { get; }

        public IfStatement(Condition cond, Statement[] ifStatements, Statement[] elseStatements)
        {
            Cond = cond ?? throw new ArgumentNullException(nameof(cond));
            IfStatements = ifStatements ?? throw new ArgumentNullException(nameof(ifStatements));
            ElseStatements = elseStatements ?? throw new ArgumentNullException(nameof(elseStatements));
        }
    }
}