using System.Collections.Generic;
using System.IO;

namespace Iridio.Preprocessing
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