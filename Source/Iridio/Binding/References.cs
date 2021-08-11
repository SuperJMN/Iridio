using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Iridio.Tokenization;

namespace Iridio.Binding
{
    public static class References
    {
        public static IEnumerable<string> FromString(string str)
        {
            var matches = Regex.Matches(str, $"{{({Tokenizer.IdentifierRegex})}}");
            return matches.Select(match => match.Groups[1].Value);
        }
    }
}