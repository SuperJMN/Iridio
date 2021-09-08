using CSharpFunctionalExtensions;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public interface IBoundNode
    {
        void Accept(IBoundNodeVisitor visitor);
        public Maybe<Position> Position { get; }
    }
}