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
        private readonly ITextFileFactory fileFactory;
        private readonly IDirectoryContext directoryContext;

        public Preprocessor(ITextFileFactory fileFactory, IDirectoryContext directoryContext)
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
}