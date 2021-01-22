using Iridio.Parsing.Model;
using Iridio.Tokenization;
using Superpower;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Parsing
{
    public class Parser : IParser
    {
        public Either<ParsingError, IridioSyntax> Parse(string source)
        {
            try
            {
                var tokenization = Tokenizer.Create().Tokenize(source);
                var parsed = ParserDefinitions.Parser.Parse(tokenization);
                return Either.Success<ParsingError, IridioSyntax>(parsed);
            }
            catch (ParseException e)
            {
                return Either.Error<ParsingError, IridioSyntax>(new ParsingError(e.ToString()));
            }
        }
    }
}