using System.Collections.Generic;
using System.IO;
using Iridio.Preprocessing;

namespace Iridio
{
    public class TextFile : ITextFile
    {
        private readonly string path;

        public TextFile(string path)
        {
            this.path = path;
        }

        public IEnumerable<string> Lines()
        {
            return File.ReadLines(path);
        }
    }
}