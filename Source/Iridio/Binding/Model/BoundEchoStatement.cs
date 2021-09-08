using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundEchoStatement : BoundStatement
    {
        public string Message { get; }

        public BoundEchoStatement(string message, Position position) : base(position)
        {
            Message = message;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}