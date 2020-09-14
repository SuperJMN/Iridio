namespace SimpleScript.Parsing.Model
{
    public class FunctionDeclaration : ISyntax
    {
        public string Name { get; }
        public Block Block { get; }

        public FunctionDeclaration(string name, Block block)
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