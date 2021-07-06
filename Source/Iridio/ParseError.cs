using System.Linq;
using Iridio.Parsing;
using MoreLinq;
using Zafiro.Core.Mixins;

namespace Iridio
{
    public class ParseError : CompilerError
    {
        private readonly CompilerInput compilerInput;
        private readonly ParsingError error;

        public ParseError(ParsingError error, CompilerInput compilerInput)
        {
            this.compilerInput = compilerInput;
            this.error = error;
        }

        public Location Location => Location.From(error.Position, compilerInput);

        public string Message
        {
            get
            {
                return error.Message
                    .SkipUntil(c => c == ')')
                    .Skip(1)
                    .TakeUntil(c => c == '\r')
                    .SkipLast(1)
                    .AsString()
                    .Trim();
            }
        }

        public override string ToString()
        {
            return $"Syntax error at {Location}: {Message}";
        }
    }

    public class Position
    {
        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public int Line { get; }
        public int Column { get; }
    }

    public class Location
    {
        public int Line { get; }
        public int Column { get; }
        public string Path { get; }
        public string Content { get; }

        public Location(Position position, string path, string content)
        {
            Line = position.Line;
            Column = position.Column;
            Path = path;
            Content = content;
        }

        public static Location From(Position position, CompilerInput compilerInput)
        {
            var taggedLine = compilerInput.TaggedLines[position.Line - 1];
            var location = new Location(new Position(taggedLine.Line, position.Column), taggedLine.Path,
                taggedLine.Content);
            return location;
        }

        public override string ToString()
        {
            return
                $"{nameof(Line)}: {Line}, {nameof(Column)}: {Column}, {nameof(Path)}: {Path}, {nameof(Content)}: {Content}";
        }
    }
}