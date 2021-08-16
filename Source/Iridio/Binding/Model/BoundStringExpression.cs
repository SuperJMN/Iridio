using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundStringExpression : BoundExpression
    {
        public BoundStringExpression(string str, Position position) : base(position)
        {
            String = str;
        }

        public string String { get; set; }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}