using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public interface ISyntax
    {
        void Accept(IExpressionVisitor visitor);
        public Position Position { get; }
    }
}