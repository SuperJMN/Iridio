namespace SimpleScript.Parsing.Model
{
    public class Function
    {
        public string Name { get; }
        public Block Block { get; }

        public Function(string name, Block block)
        {
            Name = name;
            Block = block;
        }
    }
}