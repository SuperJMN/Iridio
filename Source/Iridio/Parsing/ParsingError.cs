using Iridio.Core;

namespace Iridio.Parsing
{
    public class ParsingError
    {
        public Position Position { get; }

        public ParsingError(Position position, string message)
        {
            Position = position;
            Message = message;
        }

        public string Message { get; }

        public override string ToString()
        {
            return $"Error {Message}";
        }
    }
}