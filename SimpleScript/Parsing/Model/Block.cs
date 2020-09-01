namespace SimpleScript.Parsing.Model
{
    public class Block
    {
        public Statement[] Statements { get; }

        public Block(Statement[] statements)
        {
            Statements = statements;
        }
    }
}