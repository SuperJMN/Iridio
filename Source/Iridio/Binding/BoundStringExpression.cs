using SimpleScript.Binding.Model;

namespace SimpleScript.Binding
{
    public class BoundStringExpression : BoundExpression
    {
        public BoundStringExpression(string str)
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