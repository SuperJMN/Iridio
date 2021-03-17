using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Iridio.Parsing;
using Xunit;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Tests
{
    public class EnhancedParserTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Verify(string testName, string input, string expected)
        {
            var sut = new Parser();
            var parse = sut.Parse(input);
            var result = parse
                .MapRight(script => script.Stringyfy())
                .Handle(error => error.Message);

            result.Should().Be(expected);
        }

        public static IEnumerable<object[]> TestData()
        {
            return Directory.GetFiles("TestData\\Inputs").Join(Directory.GetFiles("TestData\\Expectations"),
                Path.GetFileName, Path.GetFileName, (i, e) =>
                {
                    return new object[]
                    {
                        Path.GetFileNameWithoutExtension(i),
                        File.ReadAllText(i),
                        File.ReadAllText(e)
                    };
                } );
        }
    }
}