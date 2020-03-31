using Superpower;
using Xunit;

namespace ScriptParser
{
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
        [InlineData("Flipe(a,b)")]
        [InlineData("A = 123")]
        public void Sentence(string source)
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
// Hola tío
b = Call(a,b,c);
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