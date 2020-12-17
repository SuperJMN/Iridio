using System.Collections.Generic;
using Iridio.Common;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public IEnumerable<BoundStatement> Statements { get; }

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