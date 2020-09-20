using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Iridio.Parsing;
using Xunit;
using Zafiro.Core.FileSystem;

namespace Iridio.Tests
{
    public class PreprocessorTests
    {
        [Theory]
        [InlineData("#include file1.rdo\r\nTwo", "One\r\nTwo")]
        [InlineData("#include file1.rdo\r\n#include Subdir\\file2.rdo\r\nFour", "One\r\nTwo\r\nThree\r\nFour")]
        public void Verify2(string input, string expected)
        {
            var sut = CreateSut(input);

            var result = sut.Process("main.rdo");

            result.Should().Be(expected);
        }

        private IPreprocessor CreateSut(string mainContent)
        {
            var mock = new TestingFileSystemOperations(mainContent);
            return new Preprocessor(mock);
        }

        private class TestingFileSystemOperations : IFileSystemOperations
        {
            private readonly ICollection<string> workingDirsHistory = new List<string>() { "" };

            private IDictionary<string, string> fileSystem;

            public TestingFileSystemOperations(string mainContent)
            {
                fileSystem = new Dictionary<string, string>()
                {
                    { "main.rdo", mainContent },
                    { "file1.rdo", "One" },
                    { "Subdir\\file2.rdo", "#include file3.rdo\r\nThree" },
                    { "Subdir\\file3.rdo", "Two" },
                };

            
            }

            public Task Copy(string source, string destination, CancellationToken cancellationToken = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public Task CopyDirectory(string source, string destination, string fileSearchPattern = null,
                CancellationToken cancellationToken = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public Task DeleteDirectory(string path)
            {
                throw new NotImplementedException();
            }

            public bool DirectoryExists(string path)
            {
                throw new NotImplementedException();
            }

            public bool FileExists(string path)
            {
                throw new NotImplementedException();
            }

            public void CreateDirectory(string path)
            {
                throw new NotImplementedException();
            }

            public void EnsureDirectoryExists(string directoryPath)
            {
                throw new NotImplementedException();
            }

            public Task DeleteFile(string filePath)
            {
                throw new NotImplementedException();
            }

            public string GetTempFileName()
            {
                throw new NotImplementedException();
            }

            public string ReadAllText(string path)
            {
                var key = Path.Combine(WorkingDirectory, path);
                return fileSystem[key];
            }

            public void WriteAllText(string path, string text)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<string> QueryDirectory(string root, Func<string, bool> selector = null)
            {
                throw new NotImplementedException();
            }

            public Stream OpenForWrite(string path)
            {
                throw new NotImplementedException();
            }

            public string GetTempDirectoryName()
            {
                throw new NotImplementedException();
            }

            public Stream OpenForRead(string path)
            {
                throw new NotImplementedException();
            }

            public string WorkingDirectory
            {
                get => workingDirsHistory.LastOrDefault();
                set => workingDirsHistory.Add(value);
            }
        }
    }


}