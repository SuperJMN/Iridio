namespace SimpleScript.Ast.Model
{
    internal class EchoStatement : Statement
    {
        public string Message { get; }

        public EchoStatement(string message)
        {
            Message = message;
        }
    }
}