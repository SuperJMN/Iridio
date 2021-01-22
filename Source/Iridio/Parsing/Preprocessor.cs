using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Iridio.Common.Utils;
using Zafiro.Core.FileSystem;

namespace Iridio.Parsing
{
    public class Preprocessor : IPreprocessor
    {
        private readonly IFileSystemOperations fileSystemOperations;

        public Preprocessor(IFileSystemOperations fileSystemOperations)
        {
            this.fileSystemOperations = fileSystemOperations;
        }

        public string Process(string path)
        {
            var directoryName = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();
            
            var newDir = Path.Combine(fileSystemOperations.WorkingDirectory, directoryName);
            var file = Path.GetFileName(path);

            using (new DirectorySwitch(fileSystemOperations, newDir))
            {
                var result = from line in fileSystemOperations.ReadAllText(file).Lines()
                where !IsComment(line)
                let processed = ExpandIfNeeded(line)
                select processed;

                return string.Join(Environment.NewLine, result);
            }
        }

        private bool IsComment(string line)
        {
            return Regex.IsMatch(line, @"(?<!\S)\s*(//.*)");
        }

        private string ExpandIfNeeded(string line)
        {
            var match = Regex.Match(line, @"#include\s+(.*)");
            if (match.Success)
            {
                var path = match.Groups[1].Value;
                return Process(path);
            }

            return line;
        }
    }
}