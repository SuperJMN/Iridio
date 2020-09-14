namespace SimpleScript.Parsing.Model
{
    public abstract class Expression : ISyntax
    {
        public abstract void Accept(IExpressionVisitor visitor);
    }

    public interface ISyntax
    {
        void Accept(IExpressionVisitor visitor);
    }
}