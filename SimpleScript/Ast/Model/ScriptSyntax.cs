namespace SimpleScript.Ast.Model
{
    public class ScriptSyntax
    {
        public Statement[] Sentences { get; }

        public ScriptSyntax(Statement[] sentences)
        {
            Sentences = sentences;
        }
    }
}