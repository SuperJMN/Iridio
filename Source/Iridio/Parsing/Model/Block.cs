using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class Block : Statement
    {
        public Statement[] Statements { get; }

        public Block(Statement[] statements, Position position) : base(position)
        {
            Statements = statements;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}