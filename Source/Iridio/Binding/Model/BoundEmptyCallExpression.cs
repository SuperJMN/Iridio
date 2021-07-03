namespace Iridio.Binding.Model
{
    internal class BoundEmptyCallExpression : BoundCallExpression
    {
        public BoundEmptyCallExpression()
        {
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
        }
    }
}