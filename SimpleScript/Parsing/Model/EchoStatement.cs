namespace SimpleScript.Parsing.Model
{
    public class EchoStatement : Statement
    {
        public string Message { get; }

        public EchoStatement(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return $"Echo: {Message}";
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}