using CSharpFunctionalExtensions;

namespace Iridio.Binding.Model
{
    public class BoundIfStatement : BoundStatement
    {
        public BoundExpression Condition { get; }
        public BoundBlock TrueBlock { get; }
        public Maybe<BoundBlock> FalseBlock { get; }

        public BoundIfStatement(BoundExpression condition, BoundBlock trueBlock, Maybe<BoundBlock> falseBlock)
        {
            Condition = condition;
            TrueBlock = trueBlock;
            FalseBlock = falseBlock;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}