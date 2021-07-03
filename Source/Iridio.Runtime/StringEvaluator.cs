using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Zafiro.Core.Mixins;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public class StringEvaluator
    {
        private const string Pattern = "(?<=(?<!{)(?:{{)*){([^{}]*)}(?=(?:}})*(?!}))";

        public Either<RunError, string> Evaluate(string str, IDictionary<string, object> dictionary)
        {
            var matches = Regex.Matches(str, Pattern);
            var refs = matches.Cast<Match>().Select(x => x.Groups[1].Value);

            var notFound = refs.Except(dictionary.Keys).ToList();
            if (notFound.Any())
            {
                return new ReferenceToUnsetVariable(notFound.ToArray());
            }

            var replace = Replace(str, dictionary);
            
            return replace
                .Replace("\"\"", "\"")
                .Replace("{{", "{")
                .Replace("}}", "}");
        }

        private string Replace(string str, IDictionary<string, object> dictionary)
        {
            var matches = Regex.Matches(str, Pattern);
            foreach (var m in matches.Cast<Match>().Reverse())
            {
                var refName = m.Groups[1].Value;
                var toReplace = m.Groups[0];
                var replacement = dictionary[refName].ToString();
                str = str.Replace(toReplace.Index, toReplace.Length, replacement);
            }

            return str;
        }
    }
}