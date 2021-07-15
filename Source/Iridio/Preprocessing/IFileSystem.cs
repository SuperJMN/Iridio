namespace Iridio.Preprocessing
{
    public interface IFileSystem : IDirectoryContext
    {
        ITextFile Get(string path);
    }
}