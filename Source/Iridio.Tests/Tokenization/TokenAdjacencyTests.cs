using System.Linq;
using FluentAssertions;
using Iridio.Tokenization;
using Xunit;

namespace Iridio.Tests.Tokenization
{
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