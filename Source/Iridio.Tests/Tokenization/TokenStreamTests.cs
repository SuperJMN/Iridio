using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Iridio.Tokenization;
using Xunit;

namespace Iridio.Tests.Tokenization
{
    public class TokenStreamTests
    {
        [Fact]
        public void Generate()
        {
            var stream = 
                TokenStream.Create(new List<SimpleToken>() {SimpleToken.Integer, SimpleToken.CloseBrace, })
                    .Take(20);
            var p = stream.ToList();

            var invalidAdjacency = p.Zip(p.Skip(1), (a, b) => (a, b))
                .Select(t => !TokenStream.InvalidTokenAdjacencyList.Contains(t));

            invalidAdjacency.Should().AllBeEquivalentTo(true);
        }
    }
}