using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Parsing;
using Xunit;

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
                .Map(script => script.Stringyfy());

            result.Should().BeSuccess()
                .And
                .Subject.Value.Should().Be(expected);
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