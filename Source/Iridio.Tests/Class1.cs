using System.Collections;
using System.Collections.Generic;
using System.Reactive.Joins;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

//  This is it: "(?<!{)({{)?{([^{}]+)}(?(1)}})(?!})"

namespace Iridio.Tests
{
    public class StringReplacerTests
    {
        [Fact]
        public void Given_simple_input_the_correct_output_should_be_the_expected()
        {
            var pattern = "(?<=(?<!{)(?:{{)*){([^{}]*)}(?=(?:}})*(?!}))";
            var input = "Hello {replaced}, {{non_replaced}}, {{{replaced_between_braces}}}";
            
            var actual = input.ReplaceWithRegex(pattern, match =>
            {
                var dict = new Dictionary<string, string>
                {
                    {"replaced", "mate"},
                    {"replaced_between_braces", "boy!"},
                    {"non_replaced", "will not appear on resulting string"}, {"", ""},
                };

                var toReplace = match.Groups[1].Value;
                return new Replacement(dict[toReplace], match.Groups[0]);
            });

            var expected = "Hello mate, {{non_replaced}}, {{boy!}}";

            actual.Should().Be(expected);
        }
    }

    public class Replacement
    {
        public string String { get; }
        public Group Range { get; }

        public Replacement(string @string, Group range)
        {
            String = @string;
            Range = range;
        }
    }
}