using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using SimpleScript.Parsing;
using SimpleScript.Tokenization;
using Superpower;
using Xunit;

namespace SimpleScript.Tests
{
    public class EnhancedParserTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Verify(string input, string expected)
        {
            var sut = new EnhancedParser();
            var parse = sut.Parse(input);
            var result = parse
                .MapSuccess(script => script.ToString())
                .Handle(error => error.Message);

            result.Should().Be(expected);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                File.ReadAllText("TestData\\Inputs\\TextFile1.txt"),
                File.ReadAllText("TestData\\Expectations\\TextFile1.txt")
            };

            yield return new object[]
            {
                File.ReadAllText("TestData\\Inputs\\TextFile2.txt"),
                File.ReadAllText("TestData\\Expectations\\TextFile2.txt")
            };

        }
    }

    public class ParserTests
    {
        [Fact]
        public void Text()
        {
            var tokenList = Tokenizer.Create().Tokenize(@"""Hola tío""");
            var tokenizer = SimpleParser.TextParameter.Parse(tokenList);
        }

        [Theory]
        [InlineData(@"Call(a,b)")]
        [InlineData(@"Call(""Saludos"", cordiales, Flipe(a,b))")]
        public void Call(string source)
        {
            var tokenList = Tokenizer.Create().Tokenize(source);
            SimpleParser.CallSentence.Parse(tokenList);
        }

        //[Fact]
        //[InlineData(@"Flipe(a,b)")]
        //public void Sentence(string source)
        //{
        //    var tokenList = Tokenizer.Create().Tokenize(source);
        //    SimpleParser.Sentence.Parse(tokenList);
        //}

        [Theory]
        [InlineData("Flipe(a,b);")]
        [InlineData("A = 123;")]
        [InlineData(@"!""c:\myscript.txt"";")]
        public void Sentence(string source)
        {
            var tokenList = Tokenizer.Create().Tokenize(source);
            SimpleParser.Sentence.Parse(tokenList);
        }

        [Theory]
        [InlineData("Flipe(a,b)")]
        [InlineData("A = 123")]
        public void Expression(string source)
        {
            var tokenList = Tokenizer.Create().Tokenize(source);
            SimpleParser.Expression.Parse(tokenList);
        }

        [Theory]
        [InlineData("A = 123")]
        public void AssignmentSentence(string source)
        {
            var tokenList = Tokenizer.Create().Tokenize(source);
            SimpleParser.AssignmentSentence.Parse(tokenList);
        }

        [Theory]
        [InlineData(@"a = 1;
b = Call(a,b,c);
<Section>
Call(""C:\Windows"",1);
Partition(""Hola"",1);")]
        [InlineData("a = 4;")]
        [InlineData("Flipe(a,b);")]
        [InlineData("Flipe(a,b);a=3;")]
        [InlineData("a = 3;Flipe(a,b);")]
        public void Script(string source)
        {
            var tokenList = Tokenizer.Create().Tokenize(source);
            SimpleParser.SimpleScript.Parse(tokenList);
        }
    }
}