namespace SimpleScript.Binding.Model
{
    public abstract class BoundExpression : IBoundNode
    {
        public abstract void Accept(IBoundNodeVisitor visitor);
    }
}