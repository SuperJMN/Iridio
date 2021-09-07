using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class Block : ISyntax
    {
        public Statement[] Statements { get; }
        public Position Position { get; }

        public Block(Statement[] statements, Position position)
        {
            Statements = statements;
            Position = position;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}