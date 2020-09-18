using Iridio.Parsing.Model;

namespace Iridio
{
    public interface IParser
    {
        ScriptSyntax Parse(string source);
    }
}