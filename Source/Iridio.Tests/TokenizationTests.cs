using System.IO;
using FluentAssertions;
using Iridio.Tokenization;
using Xunit;

namespace Iridio.Tests
{
    public class TokenizationTests
    {
        [Fact]
        public void TokenizeTest()
        {
            var sut = Tokenizer.Create();
            var input = File.ReadAllText("D:\\Repos\\WOA-Project\\Deployment-Feed\\Devices\\Lumia\\950s\\Cityman\\Main.txt");
            var result = sut.Tokenize(input);
        }
    }
}