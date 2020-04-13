using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public interface IParser
    {
        ScriptSyntax Parse(string source);
    }
}