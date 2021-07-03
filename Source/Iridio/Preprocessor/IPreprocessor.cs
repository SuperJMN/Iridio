namespace Iridio.Parsing
{
    public interface IPreprocessor
    {
        CompilerInput Process(string path);
    }
}