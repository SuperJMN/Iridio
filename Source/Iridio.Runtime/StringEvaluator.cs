using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Iridio.Runtime.ReturnValues;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public class StringEvaluator
    {
        private const string Pattern = "(?<=(?<!{)(?:{{)*){([^{}]*)}(?=(?:}})*(?!}))";

        public Either<RuntimeErrors, string> Evaluate(string str, IDictionary<string, object> dictionary)
        {
            var matches = Regex.Matches(str, Pattern);
            var refs = matches.Cast<Match>().Select(x => x.Groups[1].Value);

            var notFound = refs.Except(dictionary.Keys);
            if (notFound.Any())
            {
                var errors = notFound.Select(nf => new ReferenceToUnsetVariable(nf));
                return new RuntimeErrors(errors);
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
                str = TextMixin.Replace(str, toReplace.Index, toReplace.Length, replacement);
            }

            return str;
        }
    }
}