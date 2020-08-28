namespace SimpleScript.Binding
{
    internal class BoundEchoStatement : BoundStatement
    {
        public string Message { get; }

        public BoundEchoStatement(string message)
        {
            Message = message;
        }
    }
}