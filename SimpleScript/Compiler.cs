using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SimpleScript.Ast.Model;
using Zafiro.Core.FileSystem;

namespace SimpleScript
{
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly IFileSystemOperations fileSystem;

        public Compiler(IParser parser, IFileSystemOperations fileSystem)
        {
            this.parser = parser;
            this.fileSystem = fileSystem;
        }

        public Script Compile(string path)
        {
            return new Script(GetDeclarations(path).ToList(), GetStatements(path).Where(x => !(x is ScriptCallStatement)).ToList());
        }

        private IEnumerable<Declaration> GetDeclarations(string path)
        {
            var parsed = parser.Parse(fileSystem.ReadAllText(path));

            using (new DirectorySwitch(fileSystem, Path.GetDirectoryName(path)))
            {
                var fromChildren = parsed.Statements
                    .OfType<ScriptCallStatement>()
                    .SelectMany(statement => GetDeclarations(statement.Path));
                return parsed.Header.Declarations.Select(d => d).Concat(fromChildren.ToList());
            }
        }

        private IEnumerable<Statement> GetStatements(string path)
        {
            var syntax = parser.Parse(fileSystem.ReadAllText(path));

            using (new DirectorySwitch(fileSystem, Path.GetDirectoryName(path)))
            {
                return syntax.Statements.SelectMany(Deflate).ToList();
            }
        }

        private IEnumerable<Statement> Deflate(Statement statement)
        {
            if (statement is ScriptCallStatement e)
            {
                return GetStatements(e.Path);
            }

            return new[] { statement };
        }
    }
}