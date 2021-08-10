using System;
using Serilog;

namespace Iridio.Preprocessing
{
    public class DirectorySwitch : IDisposable
    {
        private readonly System.IO.Abstractions.IFileSystem fileSystem;
        private readonly string oldDirectory;

        public DirectorySwitch(System.IO.Abstractions.IFileSystem fileSystem, string directory)
        {
            this.fileSystem = fileSystem;
            Log.Debug("Switching to " + directory);
            oldDirectory = fileSystem.Directory.GetCurrentDirectory();
            fileSystem.Directory.SetCurrentDirectory(directory);
        }

        public void Dispose()
        {
            Log.Debug("Returning to " + oldDirectory);
            fileSystem.Directory.SetCurrentDirectory(oldDirectory);
        }
    }
}