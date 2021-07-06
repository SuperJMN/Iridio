using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Parsing;
using Xunit;

namespace Iridio.Tests.Parsing
{
    public class ParserTests
    {
        [Theory]
        [InlineData("Main { }")]
        [InlineData("Main { if (a) { } }")]
        [InlineData("Main { if (a && b) { } }")]
        [InlineData("Main { a = b && c; }")]
        [InlineData("Main { 'Hi' }")]
        public void Input_matches_output(string input)
        {
            var sut = new Parser();
            var result = sut.Parse(input)
                .Map(x => x.Stringyfy());
            result.Should().BeSuccess().And.Subject.Value.RemoveWhitespace().Should().Be(input.RemoveWhitespace());
        }
    }
}