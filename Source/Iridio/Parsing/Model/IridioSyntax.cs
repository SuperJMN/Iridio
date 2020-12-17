namespace Iridio.Parsing.Model
{
    public class IridioSyntax : ISyntax
    {
        public Procedure[] Procedures { get; }

        public IridioSyntax(Procedure[] procedures)
        {
            Procedures = procedures;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}