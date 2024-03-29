using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing.Model;

namespace Iridio.Core
{
    public static class FormattingExtensions
    {
        public static string Stringyfy(this ISyntax node)
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