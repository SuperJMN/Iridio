using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public abstract class Statement : ISyntax
    {
        protected Statement(Position position)
        {
            Position = position;
        }

        public abstract void Accept(IExpressionVisitor visitor);
        public Position Position { get; }
    }
}