using System;
using Serilog;

namespace Iridio.Preprocessing
{
    public class DirectorySwitch : IDisposable
    {
        private readonly IDirectoryContext directoryContext;
        private readonly string oldDirectory;

        public DirectorySwitch(IFileSystem directoryContext, string directory)
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
}