namespace Iridio.Parsing.Model
{
    public class EnhancedScript : ISyntax
    {
        public Header Header { get; }
        public FunctionDeclaration[] Functions { get; }

        public EnhancedScript(Header header, FunctionDeclaration[] functions)
        {
            Header = header;
            Functions = functions;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}