namespace Iridio.Parsing.Model
{
    public class Block : Statement
    {
        public Statement[] Statements { get; }

        public Block(Statement[] statements)
        {
            Statements = statements;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}