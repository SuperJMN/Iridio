using System.Text.RegularExpressions;

namespace Iridio.Parsing
{
    public static class RegexMixin
    {
        public static string RegexReplace(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement);
        }
    }
}