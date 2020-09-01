using System;
using Optional;

namespace SimpleScript.Parsing.Model
{
    internal class IfStatement : Statement
    {
        public Condition Cond { get; }
        public Block IfStatements { get; }
        public Option<Block> ElseStatements { get; }

        public IfStatement(Condition cond, Block ifStatements, Option<Block> elseStatements)
        {
            Cond = cond ?? throw new ArgumentNullException(nameof(cond));
            IfStatements = ifStatements ?? throw new ArgumentNullException(nameof(ifStatements));
            ElseStatements = elseStatements;
        }
    }
}