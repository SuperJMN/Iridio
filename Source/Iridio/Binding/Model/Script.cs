using System.Collections.Generic;
using Iridio.Tokenization;
using Superpower.Model;

namespace Iridio.Binding.Model
{
    public class Script : IBoundNode
    {
        public IEnumerable<BoundProcedure> Procedures { get; }

        public Script(IEnumerable<BoundProcedure> procedures)
        {
            Procedures = procedures;
        }

        public override string ToString()
        {
            return string.Join("\n", Procedures);
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Token<SimpleToken> Token { get; set; }
    }
}