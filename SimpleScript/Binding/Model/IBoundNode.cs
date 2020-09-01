namespace SimpleScript.Binding.Model
{
    public interface IBoundNode
    {
        void Accept(IBoundNodeVisitor visitor);
    }
}