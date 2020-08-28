namespace SimpleScript.Parsing.Model
{
    internal class EchoStatement : Statement
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
    }
}