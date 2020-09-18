using Iridio.Parsing;
using Iridio.Parsing.Model;
using Iridio.Tokenization;
using Superpower;

namespace Iridio
{
    public class Parser : IParser
    {
        public ScriptSyntax Parse(string source)
        {
            var tokenizer = Tokenizer.Create().Tokenize(source);
            var script = SimpleParser.SimpleScript.Parse(tokenizer);
            return script;
        }
    }
}