using System;
using System.Collections.Generic;
using System.Linq;
using Iridio.Tests.Tokenization;
using Iridio.Tokenization;

namespace Iridio.Tests
{
    static class TokenStream
    {
        private static readonly Random Random = new Random();
        
        private static readonly HashSet<(SimpleToken, SimpleToken)> InvalidTokenAdjacency = new HashSet<(SimpleToken, SimpleToken)>()
        {
            (SimpleToken.Integer, SimpleToken.Integer),
            (SimpleToken.Integer, SimpleToken.Double),
            (SimpleToken.Double, SimpleToken.Integer),
            (SimpleToken.Double, SimpleToken.Double),
            (SimpleToken.Text, SimpleToken.Double),
            (SimpleToken.Double, SimpleToken.Text),
            (SimpleToken.Integer, SimpleToken.Text),
            (SimpleToken.Text, SimpleToken.Integer),
            (SimpleToken.Text, SimpleToken.Text),};
        
        public static IEnumerable<SimpleToken> Create(IList<SimpleToken> tokenList)
        {
            return MoreLinq.MoreEnumerable.Random(Random, tokenList.Count)
                .Select(n => tokenList[n])
                .Correlate((l, r) => !InvalidTokenAdjacency.Contains((l, r)));
        }

        public static IReadOnlyCollection<(SimpleToken, SimpleToken)> InvalidTokenAdjacencyList =>
            InvalidTokenAdjacency.ToList().AsReadOnly();
    }
}