using System;
using System.IO;

namespace Iridio.Preprocessing
{
    public class DirectoryContext : IDisposable
    {
        private readonly DirectorySwitch dirChange;

        public DirectoryContext(IFileSystem fileSystem, string path)
        {
            var dir = Path.GetDirectoryName(path) ?? throw new InvalidOperationException();
            var finalDir = Path.Combine(fileSystem.WorkingDirectory, dir);
            FileName = Path.GetFileName(path);
            dirChange = new DirectorySwitch(fileSystem, finalDir);
        }

        public string FileName { get; }

        public void Dispose()
        {
            dirChange?.Dispose();
        }
    }
}