using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class BoundScript : IBoundNode
    {
        public BoundFunctionDeclaration StartupFunction { get; }
        public IEnumerable<BoundFunctionDeclaration> Functions { get; }
        public BoundHeader Header { get; set; }

        public BoundScript(BoundFunctionDeclaration startupFunction, IEnumerable<BoundFunctionDeclaration> functions)
        {
            StartupFunction = startupFunction;
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