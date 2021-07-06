namespace Iridio.Preprocessing
{
    public class TextFileFactory : ITextFileFactory
    {
        public ITextFile Get(string path)
        {
            return new TextFile(path);
        }
    }
}