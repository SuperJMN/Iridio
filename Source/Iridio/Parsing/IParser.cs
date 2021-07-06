using CSharpFunctionalExtensions;
using Iridio.Parsing.Model;

namespace Iridio.Parsing
{
    public interface IParser
    {
        Result<IridioSyntax, ParsingError> Parse(string source);
    }
}