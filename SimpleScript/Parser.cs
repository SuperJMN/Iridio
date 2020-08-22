using System;
using SimpleScript.Ast;
using SimpleScript.Ast.Model;
using Superpower;
using Zafiro.Core.Patterns;

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

    public class EnhancedParser : IEnhancedParser
    {
        public Either<ParsingError, EnhancedScript> Parse(string source)
        {
            var tokenizer = Tokenizer.Create().Tokenize(source);
            try
            {
                return EnhancedParsers.Parser.Parse(tokenizer);
            }
            catch (ParseException e)
            {
                return new ParsingError(e.ToString());
            }
        }
    }

    public class ParsingError
    {
        public string Message { get; }

        public ParsingError(string message)
        {
            Message = message;
        }
    }

    public interface IEnhancedParser
    {
        Either<ParsingError, EnhancedScript> Parse(string source);
    }
}