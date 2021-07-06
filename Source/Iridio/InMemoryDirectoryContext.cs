using System.IO;
using Iridio.Preprocessing;

namespace Iridio
{
    public class DirectoryContext : IDirectoryContext
    {
        public string WorkingDirectory
        {
            get => Directory.GetCurrentDirectory();
            set => Directory.SetCurrentDirectory(value);
        }
    }
}