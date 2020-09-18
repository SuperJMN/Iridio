using System.Collections.Generic;
using System.Linq;
using Iridio.Parsing.Model;

namespace Iridio
{
    public class Script
    {
        public ICollection<Declaration> Declarations { get; }
        public ICollection<Statement> Statements { get; }

        public Script(ICollection<Declaration> declarations, ICollection<Statement> statements)
        {
            Declarations = declarations;
            Statements = statements;
        }

        public override string ToString()
        {
            var declarations = $"{string.Join("\n", Declarations.Select(x => x.ToString()))}";
            var statements = $"{string.Join("\n", Statements.Select(x => x.ToString()))}";
            return $"Header:\n{declarations}\nStatements:\n{statements}";
        }
    }
}