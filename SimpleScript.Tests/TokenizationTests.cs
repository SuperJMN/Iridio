using SimpleScript.Tokenization;
using Xunit;

namespace SimpleScript.Tests
{
    public class TokenizationTests
    {
        [Fact]
        public void Test2()
        {
            var tokenizer = Tokenizer.Create();
            var result = tokenizer.Tokenize("a = 1;\r\n// Hola tío\r\nb = Call(a,b,c);\r\n<Section>\r\nCall(\"C:\\Windows\",1);\r\nPartition(\"Hola\",1);");
        }
    }
}
