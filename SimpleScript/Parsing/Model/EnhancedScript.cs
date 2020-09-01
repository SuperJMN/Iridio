namespace SimpleScript.Parsing.Model
{
    public class EnhancedScript
    {
        public Header Header { get; }
        public FunctionDeclaration[] Functions { get; }

        public EnhancedScript(Header header, FunctionDeclaration[] functions)
        {
            Header = header;
            Functions = functions;
        }
    }
}