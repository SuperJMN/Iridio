using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Iridio.Tokenization;
using Xunit;
using Tokenizer = Iridio.Tokenization.Tokenizer;

namespace Iridio.Tests.Tokenization
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
}
