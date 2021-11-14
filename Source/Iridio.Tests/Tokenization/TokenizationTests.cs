using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Iridio.Tokenization;
using Xunit;
using static Iridio.Tokenization.SimpleToken;

namespace Iridio.Tests.Tokenization
{
    public class TokenizationTests
    {
        [Fact]
        public void Tokenize_should_succeed()
        {
            var sut = Tokenizer.Create();
            var input = File.ReadAllText("TestData\\Inputs\\RealScript.txt");
            sut.Tokenize(input);
        }

        [Theory]
        [InlineData("12", Integer)]
        [InlineData("12D", Double)]
        [InlineData("12d", Double)]
        [InlineData("3.14d", Double)]
        [InlineData("\"Hello boy\"", Text)]
        [InlineData("\"Hello \"\"boy\"\"\"", Text)]
        [InlineData("{", OpenBrace)]
        [InlineData("}", CloseBrace)]
        [InlineData("'Comment'", Echo)]
        [InlineData("\"This is a string with 'single quotes'\"", Text)]
        [InlineData("dpp = \"Disk={Disk}, Name='DPP'\";", Identifier, Equal, Text, Semicolon)]
        public void Tokenize(string input, params SimpleToken[] tokens)
        {
            var sut = Tokenizer.Create();

            var tokenList = sut.Tokenize(input);

            tokenList.Select(token1 => token1.Kind).Should().BeEquivalentTo(tokens);
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
