using Iridio.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Parsing
{
    public interface IParser
    {
        Either<ParsingError, IridioSyntax> Parse(string source);
    }
}