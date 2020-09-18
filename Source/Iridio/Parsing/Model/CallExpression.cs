using System.Linq;

namespace Iridio.Parsing.Model
{
    public class CallExpression : Expression
    {
        public string Name { get; }
        public Expression[] Parameters { get; }

        public CallExpression(string name, Expression[] parameters)
        {
            Name = name;
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