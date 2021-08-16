using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Zafiro.Core.Mixins;

namespace Iridio.Runtime
{
    public class StringEvaluator
    {
        private const string Pattern = "(?<=(?<!{)(?:{{)*){([^{}]*)}(?=(?:}})*(?!}))";

        public Result<string, MissingReferences> Evaluate(string str, IDictionary<string, object> dictionary)
        {
            var matches = Regex.Matches(str, Pattern);
            var refs = matches.Select(x => x.Groups[1].Value);

            var notFound = refs.Except(dictionary.Keys).ToList();
            if (notFound.Any())
            {
                return Result.Failure<string, MissingReferences>(new MissingReferences(notFound));
            }

            var replace = Replace(str, dictionary);
            
            return replace
                .Replace("\"\"", "\"")
                .Replace("{{", "{")
                .Replace("}}", "}");
        }

        private static string Replace(string str, IDictionary<string, object> dictionary)
        {
            var matches = Regex.Matches(str, Pattern);
            foreach (var m in matches.Reverse())
            {
                var refName = m.Groups[1].Value;
                var toReplace = m.Groups[0];
                var replacement = dictionary[refName].ToString();
                str = str.Replace(toReplace.Index, toReplace.Length, replacement);
            }

            return str;
        }

        public class MissingReferences : HashSet<string>
        {
            public MissingReferences(List<string> notFound) : base(notFound)
            {
            }
        }
    }
}