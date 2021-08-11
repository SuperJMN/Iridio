using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Iridio.Preprocessing
{
    public class Line : ValueObject
    {
        public string Content { get; }
        public Maybe<string> Path { get; }
        public int Number { get; }

        public bool IsComment => Regex.IsMatch(Content, @"(?<!\S)\s*(//.*)");

        public Line(string content, int number, Maybe<string> path)
        {
            Content = content;
            Path = path;
            Number = number;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Path;
            yield return Content;
            yield return Number;
        }

        public override string ToString()
        {
            return $"Line {Number}: {Content}";
        }
    }
}