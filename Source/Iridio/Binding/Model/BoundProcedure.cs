using System;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundProcedure : IBoundNode
    {
        public string Name { get; }
        public BoundBlock Block { get; }
        public Position Position { get; }

        public BoundProcedure(string name, BoundBlock block, Position position)
        {
            Name = name;
            Block = block;
            Position = position;
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