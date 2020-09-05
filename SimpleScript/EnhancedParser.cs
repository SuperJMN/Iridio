using SimpleScript.Parsing;
using SimpleScript.Parsing.Model;
using SimpleScript.Tokenization;
using Superpower;
using Zafiro.Core.Patterns;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript
{
    public class EnhancedParser : IEnhancedParser
    {
        public Either<ParsingError, EnhancedScript> Parse(string source)
        {
            var tokenization = Tokenizer.Create().Tokenize(source);
            try
            {
                return EnhancedParsers.Parser.Parse(tokenization);
            }
            catch (ParseException e)
            {
                return new ParsingError(e.ToString());
            }
        }
    }
}