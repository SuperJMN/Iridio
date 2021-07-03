using Iridio.Tokenization;
using Superpower.Model;

namespace Iridio.Parsing.Model
{
    public class Procedure : ISyntax
    {
        public string Name { get; }
        public Block Block { get; }

        public Procedure(Token<SimpleToken> name, Block block)
        {
            Name = name.ToStringValue();
            Block = block;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}