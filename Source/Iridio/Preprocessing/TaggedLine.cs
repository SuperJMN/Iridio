using System.Text.RegularExpressions;

namespace Iridio.Preprocessing
{
    public class TaggedLine
    {
        public string Content { get; }
        public string Path { get; }
        public int Line { get; }

        public bool IsComment => Regex.IsMatch(Content, @"(?<!\S)\s*(//.*)");

        public TaggedLine(string content, string path, int line)
        {
            Content = content;
            Path = path;
            Line = line;
        }
    }
}