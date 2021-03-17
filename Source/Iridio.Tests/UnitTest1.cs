using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Iridio.Tokenization;
using Xunit;

namespace Iridio.Tests
{
    public class UnitTest1
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


    public class TokenAdjacencyTests
    {
        [Theory]
        [InlineData(SimpleToken.Text, SimpleToken.Text, false)]
        [InlineData(SimpleToken.Integer, SimpleToken.Text, false)]
        [InlineData(SimpleToken.Text, SimpleToken.Integer, false)]
        [InlineData(SimpleToken.Double, SimpleToken.Integer, false)]
        [InlineData(SimpleToken.Integer, SimpleToken.Double, false)]
        [InlineData(SimpleToken.Integer, SimpleToken.Integer, false)]
        [InlineData(SimpleToken.CloseBrace, SimpleToken.OpenBrace, true)]
        public void Token_adjacency_is_valid(SimpleToken a, SimpleToken b, bool expected)
        {
            var invalidAdjacency = TokenStream.InvalidTokenAdjacencyList;

            var isValid = !invalidAdjacency.Contains((a, b));
            isValid.Should().Be(expected);
        }
    }
}