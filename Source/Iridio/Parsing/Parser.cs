using CSharpFunctionalExtensions;
using Iridio.Parsing.Model;
using Iridio.Tokenization;
using Superpower;

namespace Iridio.Parsing
{
    public class Parser : IParser
    {
        public Result<IridioSyntax, ParsingError> Parse(string source)
        {
            try
            {
                var tokenization = Tokenizer.Create().Tokenize(source);
                var parsed = ParserDefinitions.Parser.Parse(tokenization);
                return Result.Success<IridioSyntax, ParsingError>(parsed);
            }
            catch (ParseException e)
            {
                return Result.Failure<IridioSyntax, ParsingError>(new ParsingError(e.ToString()));
            }
        }
    }
}