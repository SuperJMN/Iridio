namespace SimpleScript.Parsing.Model
{
    public class ScriptCallStatement : Statement
    {
        public string Path { get; }

        public ScriptCallStatement(string path)
        {
            Path = path;
        }

        public override string ToString()
        {
            return $"CallPath: {Path}";
        }
    }
}