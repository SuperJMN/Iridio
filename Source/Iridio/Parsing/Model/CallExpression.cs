using System.Linq;
using Iridio.Tokenization;
using Superpower.Model;

namespace Iridio.Parsing.Model
{
    public class CallExpression : Expression
    {
        public string Name { get; }
        public Expression[] Parameters { get; }

        public CallExpression(Token<SimpleToken> name, Expression[] parameters)
        {
            Name = name.ToStringValue();
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(",", Parameters.Select(x => x.ToString()))})";
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}