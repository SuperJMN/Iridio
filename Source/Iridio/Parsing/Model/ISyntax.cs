namespace Iridio.Parsing.Model
{
    public interface ISyntax
    {
        void Accept(IExpressionVisitor visitor);
    }
}