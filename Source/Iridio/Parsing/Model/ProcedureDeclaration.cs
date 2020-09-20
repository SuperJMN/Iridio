namespace Iridio.Parsing.Model
{
    public class ProcedureDeclaration : ISyntax
    {
        public string Name { get; }
        public Block Block { get; }

        public ProcedureDeclaration(string name, Block block)
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