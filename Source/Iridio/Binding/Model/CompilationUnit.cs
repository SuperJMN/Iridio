using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class CompilationUnit : IBoundNode
    {
        public BoundFunctionDeclaration StartupFunction { get; }
        public BoundHeader Header { get; }
        public IEnumerable<BoundFunctionDeclaration> Functions { get; }

        public CompilationUnit(BoundFunctionDeclaration startupFunction, BoundHeader header,
            IEnumerable<BoundFunctionDeclaration> functions)
        {
            StartupFunction = startupFunction;
            Header = header;
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