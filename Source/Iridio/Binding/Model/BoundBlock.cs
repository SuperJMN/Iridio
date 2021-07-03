using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public ICollection<BoundStatement> Statements { get; }

        public BoundBlock(ICollection<BoundStatement> statements)
        {
            Statements = statements;
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}