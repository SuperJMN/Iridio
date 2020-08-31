using System;
using Zafiro.Core.Patterns;

namespace SimpleScript.Binding.Model
{
    public class BoundFunction : IBoundNode
    {
        public string Name { get; }
        public BoundBlock Block { get; }

        public BoundFunction(string name, BoundBlock block)
        {
            Name = name;
            Block = block;
        }

        public override string ToString()
        {
            var statements = string.Join(Environment.NewLine, Block);
            return $"{Name}\r\n{{\r\n{statements}\r\n}}";
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);   
        }
    }
}