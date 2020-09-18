using Iridio.Parsing;
using Iridio.Tokenization;
using Superpower;
using Xunit;

namespace Iridio.Tests
{
    public class IndividualParserTests
    {
        [Fact]
        public void BlockTest()
        {
            var input = @"{ Call(""this"", ""and"", ""this""); }";
            var parsed = EnhancedParsers.Block.Parse(Tokenizer.Create().Tokenize(input));
        }

        [Fact]
        public void IfTest()
        {
            var input = @"if (a > b) 
    {
        c = ""123"";
    } 
    else 
    {
        d = ""444"";
    }";
            var parsed = EnhancedParsers.IfStatement.Parse(Tokenizer.Create().Tokenize(input));
        }
    }
}