using Iridio.Parsing;

namespace Iridio.Preprocessing
{
    public interface IPreprocessor
    {
        PreprocessedSource Process(string path);
    }
}