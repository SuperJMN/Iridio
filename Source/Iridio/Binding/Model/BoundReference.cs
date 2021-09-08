using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundReference : BoundExpression
    {
        public string Identifier { get; }

        public BoundReference(string identifier, Position position) : base(position)
        {
            Identifier = identifier;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}