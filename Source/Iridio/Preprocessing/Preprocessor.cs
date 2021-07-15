using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Iridio.Parsing;

namespace Iridio.Preprocessing
{
    public class Preprocessor : IPreprocessor
    {
        private readonly IFileSystem fileSystem;

        public Preprocessor(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public SourceCode Process(string path)
        {
            return new(ProcessCore(path).ToList());
        }

        private IEnumerable<Line> ProcessCore(string path)
        {
            var directoryName = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();

            var newDir = Path.Combine(fileSystem.WorkingDirectory, directoryName);
            var file = Path.GetFileName(path);

            using (new DirectorySwitch(fileSystem, newDir))
            {
                return from line in Lines(file)
                    from expanded in Expand(line)
                    select expanded;
            }
        }

        private IEnumerable<Line> Lines(string path)
        {
            return fileSystem.Get(path)
                .Lines()
                .Select((s, i) => new Line(s, path, i + 1));
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

    public interface IFileSystem : IDirectoryContext
    {
        ITextFile Get(string path);
    }
}