using System.Collections.Generic;

namespace SimpleScript.Ast.Model
{
    public class ScriptSyntax
    {
        public Header Header { get; }
        public IEnumerable<Statement> Sentences { get; }

        public ScriptSyntax(Header header, IEnumerable<Statement> sentences)
        {
            Header = header;
            Sentences = sentences;
        }
    }
}