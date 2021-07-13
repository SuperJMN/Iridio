using System;
using CSharpFunctionalExtensions;

namespace Iridio.Parsing.Model
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; }
        public Block TrueBlock { get; }
        public Maybe<Block> FalseBlock { get; }

        public IfStatement(Expression cond, Block trueBlock, Maybe<Block> falseBlock)
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