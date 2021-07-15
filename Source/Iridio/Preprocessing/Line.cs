using System.Text.RegularExpressions;

namespace Iridio.Preprocessing
{
    public class Line
    {
        public string Content { get; }
        public string Path { get; }
        public int Number { get; }

        public bool IsComment => Regex.IsMatch(Content, @"(?<!\S)\s*(//.*)");

        public Line(string content, string path, int number)
        {
            Content = content;
            Path = path;
            Number = number;
        }
    }
}