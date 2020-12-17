using System.Collections.Generic;

namespace Iridio.Parsing.Model
{
    public class ScriptSyntax
    {
        public IEnumerable<Statement> Statements { get; }

        public ScriptSyntax(IEnumerable<Statement> statements)
        {
            Statements = statements;
        }
    }
}