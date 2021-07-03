using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Iridio.Parsing;
using Serilog;

namespace Iridio.Preprocessor
{
    public class DirectorySwitch : IDisposable
    {
        private readonly IDirectoryContext directoryContext;
        private readonly string oldDirectory;

        public DirectorySwitch(IDirectoryContext directoryContext, string directory)
        {
            this.directoryContext = directoryContext;
            Log.Debug("Switching to " + directory);
            oldDirectory = directoryContext.WorkingDirectory;
            directoryContext.WorkingDirectory = directory;
        }

        public void Dispose()
        {
            Log.Debug("Returning to " + oldDirectory);
            directoryContext.WorkingDirectory = oldDirectory;
        }
    }

    public interface IDirectoryContext
    {
        string WorkingDirectory { get; set; }
    }

    public class NewPreprocessor : IPreprocessor
    {
        private readonly ITextFileFactory fileFactory;
        private readonly IDirectoryContext directoryContext;

        public NewPreprocessor(ITextFileFactory fileFactory, IDirectoryContext directoryContext)
        {
            this.fileFactory = fileFactory;
            this.directoryContext = directoryContext;
        }

        public CompilerInput Process(string path)
        {
            return new(ProcessCore(path));
        }

        private IEnumerable<TaggedLine> ProcessCore(string path)
        {
            var directoryName = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();

            var newDir = Path.Combine(directoryContext.WorkingDirectory, directoryName);
            var file = Path.GetFileName(path);

            using (new DirectorySwitch(directoryContext, newDir))
            {
                return from line in TaggedLines(file)
                    where !line.IsComment
                    from expanded in Expand(line)
                    select expanded;
            }
        }

        private IEnumerable<TaggedLine> TaggedLines(string path)
        {
            return fileFactory.Get(path)
                .Lines()
                .Select((s, i) => new TaggedLine(s, path, i + 1));
        }

        private IEnumerable<TaggedLine> Expand(TaggedLine line)
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

    public interface ITextFileFactory
    {
        ITextFile Get(string path);
    }

    public class TextFileFactory : ITextFileFactory
    {
        public ITextFile Get(string path)
        {
            return new TextFile(path);
        }
    }

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