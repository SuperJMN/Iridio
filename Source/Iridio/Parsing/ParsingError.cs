namespace Iridio.Parsing
{
    public class ParsingError
    {
        public string Message { get; }

        public ParsingError(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return $"Error {Message}";
        }
    }
}