using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SimpleScript.Ast;
using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly Func<string, string> loadSourceFromPath;

        public Compiler(IParser parser, Func<string, string> loadSourceFromPath)
        {
            this.parser = parser;
            this.loadSourceFromPath = loadSourceFromPath;
        }

        public Script Compile(string source)
        {
            var syntax = parser.Parse(source);
            var statementNodes = ToNode(syntax);
            var declarations = GetDeclarations(syntax).Distinct().ToList();

            var treeNode = new TreeNode<Statement>(null, statementNodes);
            var statements = MoreEnumerable.TraverseDepthFirst(treeNode, node => node.Children)
                .Where(node => !(node.Value is ScriptCallStatement))
                .Select(x => x.Value);
            return new Script(declarations, statements.Skip(1).ToList());
        }

        private IEnumerable<Declaration> GetDeclarations(ScriptSyntax syntax)
        {
            var declarations = syntax.Header.Declarations;
            var calls = syntax.Sentences.OfType<ScriptCallStatement>();
            var scripts = calls.Select(statement => parser.Parse(loadSourceFromPath(statement.Path)));
            var childDecl = scripts.SelectMany(GetDeclarations);
            return declarations.Concat(childDecl);
        }

        private IEnumerable<TreeNode<Statement>> ToNode(ScriptSyntax scriptSyntax)
        {
            return scriptSyntax.Sentences.Select(ToNode);
        }

        private TreeNode<Statement> ToNode(Statement statement)
        {
            var children = statement is ScriptCallStatement call
                ? ToNode(parser.Parse(loadSourceFromPath(call.Path)))
                : Enumerable.Empty<TreeNode<Statement>>();

            return new TreeNode<Statement>(statement, children.ToList());
        }
    }
}