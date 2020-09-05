using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript
{
    public interface IEnhancedParser
    {
        Either<ParsingError, EnhancedScript> Parse(string source);
    }
}