using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public interface IScriptParser
    {
        ScriptSyntax Parse(string source);
    }
}