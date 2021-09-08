using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundBlock : IBoundNode
    {
        public IReadOnlyCollection<BoundStatement> Statements { get; }

        public BoundBlock(IEnumerable<BoundStatement> statements)
        {
            Statements = statements.ToList();
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Maybe<Position> Position => Maybe<Position>.None;
    }
}