using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public IEnumerable<BoundStatement> Statements { get; }

        public BoundBlock(IEnumerable<BoundStatement> statements, Position position)
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