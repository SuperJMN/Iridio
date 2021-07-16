using Iridio.Parsing;

namespace Iridio.Preprocessing
{
    public interface IPreprocessor
    {
        SourceCode Process(string path);
    }
}