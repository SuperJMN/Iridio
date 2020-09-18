using Iridio.Parsing;
using Iridio.Parsing.Model;
using Iridio.Tokenization;
using Superpower;
using Zafiro.Core.Patterns.Either;

namespace Iridio
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