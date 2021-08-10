using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Iridio.Parsing;

namespace Iridio.Preprocessing
{
    public class Preprocessor : IPreprocessor
    {
        private readonly System.IO.Abstractions.IFileSystem fileSystem;

        public Preprocessor(System.IO.Abstractions.IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public SourceCode Process(string path)
        {
            return new(ProcessCore(path).ToList());
        }

        private IEnumerable<Line> ProcessCore(string path)
        {
            using (var pr = new DirectoryContext(fileSystem, path))
            {
                return ProcessFile(pr.FileName).ToList();
            }
        }

        private IEnumerable<Line> ProcessFile(string file)
        {
            return from line in Lines(file)
                from expanded in Expand(line)
                select expanded;
        }

        private IEnumerable<Line> Lines(string path)
        {
            return fileSystem.File.ReadAllLines(path)
                .Select((s, i) => new Line(s, i + 1, path));
        }

        private IEnumerable<Line> Expand(Line line)
        {
            var match = Regex.Match(line.Content, @"#include\s+(.*)");
            if (match.Success)
            {
                var path = match.Groups[1].Value;
                return ProcessCore(path);
            }

            return new[] {line};
        }
    }
}