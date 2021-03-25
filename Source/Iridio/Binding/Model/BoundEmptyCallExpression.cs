namespace Iridio.Binding.Model
{
    internal class BoundEmptyCallExpression : BoundCallExpression
    {
        public override void Accept(IBoundNodeVisitor visitor)
        {
        }
    }
}