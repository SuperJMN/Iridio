using SimpleScript.Parsing;
using SimpleScript.Parsing.Model;
using SimpleScript.Tokenization;
using Superpower;

namespace SimpleScript
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