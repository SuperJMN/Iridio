using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns;

namespace SimpleScript
{
    public interface IEnhancedParser
    {
        Either<ParsingError, EnhancedScript> Parse(string source);
    }
}