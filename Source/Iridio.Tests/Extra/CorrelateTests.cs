using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Iridio.Tests.Tokenization;
using Iridio.Tokenization;
using Xunit;

namespace Iridio.Tests.Extra
{
    public class CorrelateTests
    {
        [Theory]
        [InlineData(new[] { 1 }, new[] { 1 })]
        [InlineData(new[] { 1, 1 }, new[] { 1 })]
        [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
        [InlineData(new[] { 1, 1, 1 }, new[] { 1, })]
        [InlineData(new[] { 1, 1, 2 }, new[] { 1, 2 })]
        public void CorrelateTest(int[] source, int[] expected)
        {
            var list = new[] { 1 };
            source.Correlate((a, b) => a != b).Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(new[]{ SimpleToken.Integer, SimpleToken.Integer}, new[] { SimpleToken.Integer })]
        [InlineData(new[] { SimpleToken.Text, SimpleToken.Text, SimpleToken.Text }, new[] { SimpleToken.Text })]
        [InlineData(new[] { SimpleToken.CloseBrace, SimpleToken.Text, SimpleToken.Text }, new[] { SimpleToken.CloseBrace, SimpleToken.Text })]
        public void Fixed_sequence(IEnumerable<SimpleToken> input, IEnumerable<SimpleToken> expected)
        {
            var actual = input.Correlate((a, b) => !TokenStream.InvalidTokenAdjacencyList.Contains((a, b)));
            actual.Should().BeEquivalentTo(expected);
        }
    }
}