using Iridio.Binding.Model;

namespace Iridio.Binding
{
    public class BoundIdentifier : BoundExpression
    {
        public string Identifier { get; }

        public BoundIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}