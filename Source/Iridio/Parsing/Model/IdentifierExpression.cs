using Iridio.Tokenization;
using Superpower.Model;

namespace Iridio.Parsing.Model
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }

        public IdentifierExpression(Token<SimpleToken> identifier)
        {
            Identifier = identifier.ToStringValue();
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