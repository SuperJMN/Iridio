using SimpleScript.Parsing;
using SimpleScript.Parsing.Model;
using SimpleScript.Tokenization;
using Superpower;
using Zafiro.Core.Patterns;

namespace SimpleScript
{
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
}