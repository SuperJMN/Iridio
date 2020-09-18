using Iridio.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    public interface IEnhancedParser
    {
        Either<ParsingError, EnhancedScript> Parse(string source);
    }
}