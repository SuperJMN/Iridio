using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Iridio.Tests
{
    public static class TextMixin
    {
        public static string Replace(this string input, int index, int length, string replacement)
        {
            var builder = new StringBuilder();
            builder.Append(input.Substring(0, index));
            builder.Append(replacement);
            builder.Append(input[(index + length)..]);
            return builder.ToString();
        }

        public static string ReplaceWithRegex(this string input, string pattern, Func<Match, Replacement> replaceFactory)
        {
            foreach (var match in Regex.Matches(input, pattern).Reverse())
            {
                var replacement = replaceFactory(match);
                input = Replace(input, replacement.Range.Index, replacement.Range.Length, replacement.String);
            }

            return input;
        }
    }
}