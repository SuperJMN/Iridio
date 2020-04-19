using System.Collections.Generic;
using System.Linq;

namespace SimpleScript
{
    internal class TreeNode<T>
    {
        public TreeNode(T value) : this(value, Enumerable.Empty<TreeNode<T>>())
        {
        }

        public TreeNode(T value, IEnumerable<TreeNode<T>> children)
        {
            Value = value;
            Children = children;
        }

        public T Value { get; }
        public IEnumerable<TreeNode<T>> Children { get; }
    }
}