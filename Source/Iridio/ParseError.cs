using Iridio.Parsing;

namespace Iridio
{
    public class ParseError : CompilerError
    {
        public ParsingError Error { get; }

        public ParseError(ParsingError error)
        {
            Error = error;
        }

        public override string ToString()
        {
            return Error.ToString();
        }
    }
}