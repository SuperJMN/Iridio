namespace SimpleScript.Parsing.Model
{
    public class FunctionDeclaration
    {
        public string Name { get; }
        public Block Block { get; }

        public FunctionDeclaration(string name, Block block)
        {
            Name = name;
            Block = block;
        }
    }
}