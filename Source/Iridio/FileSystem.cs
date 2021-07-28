using System.IO;
using Iridio.Preprocessing;

namespace Iridio
{
    public class FileSystem : IFileSystem
    {
        public string WorkingDirectory
        {
            get => Directory.GetCurrentDirectory();
            set => Directory.SetCurrentDirectory(value);
        }

        public ITextFile Get(string path)
        {
            return new TextFile(path);
        }
    }
}