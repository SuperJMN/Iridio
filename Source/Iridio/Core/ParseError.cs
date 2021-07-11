using Iridio.Parsing;

namespace Iridio.Core
{
    public class ParseError : CompilerError
    {
        private readonly ParsingError error;

        public ParseError(ParsingError error, Location location)
        {
            this.error = error;
            Location = location;
        }

        public Location Location { get; }

        public string Message => error.Message;

        public override string ToString()
        {
            return $"Syntax error at {Location}: {Message}";
        }
    }
}