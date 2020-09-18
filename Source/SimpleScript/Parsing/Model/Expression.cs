namespace SimpleScript.Parsing.Model
{
    public abstract class Expression : ISyntax
    {
        public abstract void Accept(IExpressionVisitor visitor);
    }
}