using SimpleScript.Parsing.Model;

namespace SimpleScript.Binding.Model
{
    public class BoundIfStatement : BoundStatement
    {
        public BoundCondition Condition { get; }
        public BoundBlock TrueBlock { get; }
        public BoundBlock FalseBlock { get; }

        public BoundIfStatement(BoundCondition condition, BoundBlock trueBlock, BoundBlock falseBlock)
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