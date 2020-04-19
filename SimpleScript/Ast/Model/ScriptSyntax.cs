using System.Collections.Generic;

namespace SimpleScript.Ast.Model
{
    public class ScriptSyntax
    {
        public Header Header { get; }
        public IEnumerable<Statement> Statements { get; }

        public ScriptSyntax(Header header, IEnumerable<Statement> statements)
        {
            Header = header;
            Statements = statements;
        }
    }
}