using System.IO;
using Iridio.Tokenization;
using Xunit;

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
    }
}