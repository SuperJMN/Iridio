using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Bogus;
using DynamicData;
using FluentAssertions;
using Iridio.Tokenization;
using MoreLinq;
using MoreLinq.Extensions;
using Optional.Collections;
using Superpower.Parsers;
using Xunit;
using Tokenizer = Iridio.Tokenization.Tokenizer;

namespace Iridio.Tests
{
    public class TokenizationTests
    {
        [Fact]
        public void Tokenize_should_succeed()
        {
            var sut = Tokenizer.Create();
            var input = File.ReadAllText("TestData\\Inputs\\RealScript.txt");
            var result = sut.Tokenize(input);
        }

        [Theory]
        [InlineData("12", SimpleToken.Integer)]
        [InlineData("12D", SimpleToken.Double)]
        [InlineData("\"Hello boy\"", SimpleToken.Text)]
        [InlineData("\"Hello \"\"boy\"\"\"", SimpleToken.Text)]
        [InlineData("{", SimpleToken.OpenBrace)]
        [InlineData("}", SimpleToken.CloseBrace)]
        public void Tokenize(string input, SimpleToken token)
        {
            var sut = Tokenizer.Create();

            var tokenList = sut.Tokenize(input);

            var p = tokenList.ConsumeToken();
            p.Remainder.IsAtEnd.Should().BeTrue();
            p.Value.Kind.Should().BeEquivalentTo(token);
        }

        [Theory]
        [MemberData(nameof(GenerateFuzz))]
        public void Fuzzer(string input, IEnumerable<SimpleToken> expectedTokens)
        {
            var sut = Tokenizer.Create();

            var tokenList = sut.Tokenize(input);
            var tokens = tokenList.Select(x => x.Kind);
            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        public static IEnumerable<object[]> GenerateFuzz()
        {
            var tokenizerFuzzer = new TokenizerFuzzer();
            return Enumerable.Range(1, 50).Select(_ => tokenizerFuzzer.Generate(3))
                .Select(tuple => new object[] { tuple.Item1, tuple.Item2 });
        }
    }

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

    class TokenizerFuzzer
    {
        public TokenizerFuzzer()
        {
            var random = new Random();
            var faker = new Faker();
            dictionary = new Dictionary<SimpleToken, Func<string>>()
            {
                {SimpleToken.CloseBrace, () => "}"},
                {SimpleToken.OpenBrace, () => "{"},
                {SimpleToken.Integer, () => faker.Random.Number(1000).ToString()},
                {SimpleToken.Semicolon, () => ";"},
                {SimpleToken.Text, () => "\"" + faker.Random.AlphaNumeric(10) + "\"" },
                {SimpleToken.Double, () => random.NextDouble().ToString(CultureInfo.InvariantCulture) + "d"},
            };
        }

        private readonly IDictionary<SimpleToken, Func<string>> dictionary;


        public (string, IEnumerable<SimpleToken>) Generate(int numberOfTokens)
        {
            var tokens = TokenStream
                .Create(dictionary.Keys.ToList())
                .Take(numberOfTokens)
                .ToList();
            
            var str = string.Concat(tokens.Select(GetString));
            return (str, tokens);
        }

        private string GetString(SimpleToken simpleToken)
        {
            var value = DictionaryExtensions.GetValueOrNone(dictionary, simpleToken)
                .Map(toString => toString())
                .ValueOr("");
            return value;
        }
    }

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
