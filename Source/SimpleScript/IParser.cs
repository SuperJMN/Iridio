using SimpleScript.Parsing.Model;

namespace SimpleScript
{
    public interface IParser
    {
        ScriptSyntax Parse(string source);
    }
}