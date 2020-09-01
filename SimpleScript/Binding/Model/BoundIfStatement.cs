using Optional;
using SimpleScript.Parsing.Model;

namespace SimpleScript.Binding.Model
{
    public class BoundIfStatement : BoundStatement
    {
        public BoundCondition Condition { get; }
        public BoundBlock TrueBlock { get; }
        public Option<BoundBlock> FalseBlock { get; }

        public BoundIfStatement(BoundCondition condition, BoundBlock trueBlock, Option<BoundBlock> falseBlock)
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