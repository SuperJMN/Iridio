using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class Procedure : ISyntax
    {
        public string Name { get; }
        public Position Position { get; }
        public Block Block { get; }

        public Procedure(string name, Position position, Block block)
        {
            Name = name;
            Position = position;
            Block = block;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}