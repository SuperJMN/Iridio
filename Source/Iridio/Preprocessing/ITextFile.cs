using System.Collections.Generic;

namespace Iridio.Preprocessing
{
    public interface ITextFile
    {
        IEnumerable<string> Lines();
    }
}