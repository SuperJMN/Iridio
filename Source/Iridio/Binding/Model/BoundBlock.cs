using System.Collections.Generic;
using System.Linq;

namespace Iridio.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public IEnumerable<BoundStatement> Statements { get; }

        public BoundBlock():this(Enumerable.Empty<BoundStatement>())
        {
        }

        public BoundBlock(IEnumerable<BoundStatement> statements)
        {
            Statements = statements;
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}