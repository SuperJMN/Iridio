using FluentAssertions;
using Iridio.Parsing;
using Xunit;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Tests.Parsing
{
    public class ParserTests
    {
        [Fact]
        public void Empty_main()
        {
            var sut = new Parser();
            var result =sut.Parse("Main { }");
            var expected = result.MapRight(x => x.Stringyfy());
            expected.Should().BeEquivalentTo(Either.Success<ParsingError, string>("Main\r\n{\r\n}"));
        }

        [Fact]
        public void If_simple_condition()
        {
            var sut = new Parser();
            var result = sut.Parse("Main { if (a) { } }");
            var expected = result.MapRight(x => x.Stringyfy());
            expected.Should()
                .BeEquivalentTo(Either.Success<ParsingError, string>("Main\r\n{\r\n\tif (a)\r\n\t{\r\n\t}\r\n}"));
        }

        [Fact]
        public void If_boolean_condition()
        {
            var sut = new Parser();
            var result = sut.Parse("Main { if (a && b) { } }");
            var expected = result.MapRight(x => x.Stringyfy());
            expected.Should().BeEquivalentTo(Either.Success<ParsingError, string>("Main\r\n{\r\n\tif (a && b)\r\n\t{\r\n\t}\r\n}"));
        }

        [Fact]
        public void Boolean_assignment()
        {
            var sut = new Parser();
            var result = sut.Parse("Main { a = b && c; }");
            var expected = result.MapRight(x => x.Stringyfy());
            expected.Should().BeEquivalentTo(Either.Success<ParsingError, string>("Main\r\n{\r\n\ta = b && c;\r\n}"));
        }
    }
}