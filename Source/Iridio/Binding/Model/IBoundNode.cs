using Iridio.Core;

namespace Iridio.Binding.Model
{
    public interface IBoundNode
    {
        void Accept(IBoundNodeVisitor visitor);
        public Position Position { get; }
    }
}