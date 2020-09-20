namespace Iridio.Parsing.Model
{
    public class EnhancedScript : ISyntax
    {
        public Header Header { get; }
        public ProcedureDeclaration[] Procedures { get; }

        public EnhancedScript(Header header, ProcedureDeclaration[] procedures)
        {
            Header = header;
            Procedures = procedures;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}