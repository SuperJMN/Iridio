using System.Collections.Generic;
using System.Linq;

namespace SimpleScript
{
    internal class TreeNode<T>
    {
        public TreeNode()
        {
        }

        public TreeNode(T value, IEnumerable<TreeNode<T>> children)
        {
            Value = value;
            Children = children;
        }

        public T Value { get; set; }
        public IEnumerable<TreeNode<T>> Children { get; set; } = Enumerable.Empty<TreeNode<T>>();
    }
}