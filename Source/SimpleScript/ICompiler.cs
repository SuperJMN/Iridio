namespace SimpleScript
{
    public interface ICompiler
    {
        Script Compile(string path);
    }
}