namespace SimpleScript.Binding.Model
{
    public class BoundCallStatement : BoundStatement
    {
        public BoundCallExpression Call { get; }

        public BoundCallStatement(BoundCallExpression call)
        {
            Call = call;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}