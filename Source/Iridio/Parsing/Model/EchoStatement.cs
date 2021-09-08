using Iridio.Core;

namespace Iridio.Parsing.Model
{
    public class EchoStatement : Statement
    {
        public string Message { get; }

        public EchoStatement(string message, Position position) : base(position)
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