using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing.Model;

namespace Iridio.Tests
{
    public static class FormattingExtensions
    {
        public static string AsString(this IBoundNode node)
        {
            var visitor = new BoundNodeStringifyVisitor();
            node.Accept(visitor);
            return visitor.ToString();
        }

        public static string AsString(this ISyntax node)
        {
            var visitor = new SyntaxStringifyVisitor();
            node.Accept(visitor);
            return visitor.ToString();
        }

        public static string Flatten(this Errors list)
        {
            return string.Join(",", list);
        }
    }
}