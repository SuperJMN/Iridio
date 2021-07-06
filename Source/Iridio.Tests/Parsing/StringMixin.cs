using System.Linq;

namespace Iridio.Tests.Parsing
{
    public static class StringMixin
    {
        public static string RemoveWhitespace(this string str)
        {
            return string.Concat(str.Where(c => !char.IsWhiteSpace(c)));
        }
    }
}