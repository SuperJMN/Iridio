using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public ICollection<BoundStatement> Statements { get; }

        public BoundBlock(ICollection<BoundStatement> statements, Position position)
        {
            Statements = statements;
            Position = position;
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Position Position { get; }
    }
}