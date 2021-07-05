namespace Iridio.Preprocessing
{
    public interface ITextFileFactory
    {
        ITextFile Get(string path);
    }
}