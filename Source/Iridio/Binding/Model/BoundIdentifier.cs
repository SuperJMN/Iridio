using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundIdentifier : BoundExpression
    {
        public string Identifier { get; }

        public BoundIdentifier(string identifier, Position position) : base(position)
        {
            Identifier = identifier;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}