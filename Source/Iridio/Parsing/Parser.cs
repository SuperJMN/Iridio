using Iridio.Parsing.Model;
using Iridio.Tokenization;
using Superpower;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Parsing
{
    public class Parser : IParser
    {
        public Either<ParsingError, EnhancedScript> Parse(string source)
        {
            try
            {
                var tokenization = Tokenizer.Create().Tokenize(source);
                return EnhancedParsers.Parser.Parse(tokenization);
            }
            catch (ParseException e)
            {
                return new ParsingError(e.ToString());
            }
        }
    }
}