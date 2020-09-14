using SimpleScript.Binding;
using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Tests
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

        public static string Flatten(this ErrorList list)
        {
            return string.Join(",", list);
        }
    }
}