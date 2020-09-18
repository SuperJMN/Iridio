namespace SimpleScript.Binding.Model
{
    public class BoundEchoStatement : BoundStatement
    {
        public string Message { get; }

        public BoundEchoStatement(string message)
        {
            Message = message;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}