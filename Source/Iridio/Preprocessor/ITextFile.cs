using System.Collections.Generic;

namespace Iridio.Preprocessor
{
    public interface ITextFile
    {
        IEnumerable<string> Lines();
    }
}