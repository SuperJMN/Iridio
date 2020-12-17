namespace Iridio.Parsing.Model
{
    public class Procedure : ISyntax
    {
        public string Name { get; }
        public Block Block { get; }

        public Procedure(string name, Block block)
        {
            Name = name;
            Block = block;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}