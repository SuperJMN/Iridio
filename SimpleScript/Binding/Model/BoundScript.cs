using System.Collections.Generic;

namespace SimpleScript.Binding.Model
{
    public class BoundScript : IBoundNode
    {
        public IEnumerable<BoundFunctionDeclaration> Functions { get; }

        public BoundScript(IEnumerable<BoundFunctionDeclaration> functions)
        {
            Functions = functions;
        }

        public override string ToString()
        {
            return string.Join("\n", Functions);
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}