namespace SimpleScript.Ast.Model
{
    public class Script
    {
        public Statement[] Sentences { get; }

        public Script(Statement[] sentences)
        {
            Sentences = sentences;
        }
    }
}