using SimpleScript.Ast.Model;

namespace SimpleScript.Ast
{
    public class EnhancedScript
    {
        public Header Header { get; }
        public Function[] Functions { get; }

        public EnhancedScript(Header header, Function[] functions)
        {
            Header = header;
            Functions = functions;
        }
    }
}