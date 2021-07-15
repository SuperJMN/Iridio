using Iridio.Parsing;

namespace Iridio.Core
{
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

        public static Location From(Position position, SourceCode sourceCode)
        {
            var taggedLine = sourceCode.TaggedLines[position.Line - 1];
            var location = new Location(new Position(taggedLine.Number, position.Column), taggedLine.Path,
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