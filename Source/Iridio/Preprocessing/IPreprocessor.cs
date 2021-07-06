using Iridio.Parsing;

namespace Iridio.Preprocessing
{
    public interface IPreprocessor
    {
        CompilerInput Process(string path);
    }
}