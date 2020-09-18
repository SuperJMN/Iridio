using System;
using Optional;

namespace SimpleScript.Parsing.Model
{
    public class IfStatement : Statement
    {
        public Condition Condition { get; }
        public Block TrueBlock { get; }
        public Option<Block> FalseBlock { get; }

        public IfStatement(Condition cond, Block trueBlock, Option<Block> falseBlock)
        {
            Condition = cond ?? throw new ArgumentNullException(nameof(cond));
            TrueBlock = trueBlock ?? throw new ArgumentNullException(nameof(trueBlock));
            FalseBlock = falseBlock;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}