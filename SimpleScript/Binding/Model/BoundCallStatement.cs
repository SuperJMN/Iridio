namespace SimpleScript.Binding.Model
{
    public class BoundCallStatement : BoundStatement
    {
        public BoundCustomCallExpression Call { get; }

        public BoundCallStatement(BoundCustomCallExpression call)
        {
            Call = call;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}