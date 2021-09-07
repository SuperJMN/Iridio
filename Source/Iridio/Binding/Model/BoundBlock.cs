using System.Collections.Generic;
using System.Linq;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public IReadOnlyCollection<BoundStatement> Statements { get; }

        public BoundBlock(IEnumerable<BoundStatement> statements, Position position)
        {
            Statements = statements.ToList();
            Position = position;
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Position Position { get; }
    }
}