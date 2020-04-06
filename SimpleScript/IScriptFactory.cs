namespace SimpleScript
{
    public interface IScriptFactory
    {
        Script Load(string source);
    }
}