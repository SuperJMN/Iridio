using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }

        public IdentifierExpression(string identifier, Position position) : base(position)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return $"{Identifier}";
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}