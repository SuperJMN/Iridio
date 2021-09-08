using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class Block : ISyntax
    {
        public Statement[] Statements { get; }
        public Position Position { get; }

        public Block(Statement[] statements)
        {
            Statements = statements;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}