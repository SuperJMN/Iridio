using SimpleScript.Ast;
using Xunit;

namespace Tests
{
    public class TokenizationTests
    {
        private readonly ParserTests parserTests = new ParserTests();

        [Fact]
        public void Test2()
        {
            var tokenizer = Tokenizer.Create();
            var result = tokenizer.Tokenize("a = 1;\r\n// Hola tío\r\nb = Call(a,b,c);\r\n<Section>\r\nCall(\"C:\\Windows\",1);\r\nPartition(\"Hola\",1);");
        }
    }
}
