using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class IridioSyntax : ISyntax
    {
        public Procedure[] Procedures { get; }

        public IridioSyntax(Procedure[] procedures, Position position)
        {
            Procedures = procedures;
            Position = position;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Position Position { get; }
    }
}