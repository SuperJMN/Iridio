namespace SimpleScript.Binding.Model
{
    public abstract class BoundStatement : IBoundNode
    {
        public abstract void Accept(IBoundNodeVisitor visitor);
    }
}