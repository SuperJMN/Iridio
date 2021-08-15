using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public abstract class Expression : ISyntax
    {
        protected Expression(Position position)
        {
            Position = position;
        }

        public abstract void Accept(IExpressionVisitor visitor);
        public Position Position { get; }
    }
}