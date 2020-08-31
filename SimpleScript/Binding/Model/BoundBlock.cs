using System.Collections.Generic;

namespace SimpleScript.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public IEnumerable<BoundStatement> BoundStatements { get; }

        public BoundBlock(IEnumerable<BoundStatement> boundStatements)
        {
            BoundStatements = boundStatements;
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}