using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public class Script
    {
        public IEnumerable<IFunction> Functions { get; }
        private readonly ScriptSyntax syntax;
        private readonly IScriptRunner runner;

        public Script(ScriptSyntax syntax, IScriptRunner runner, IEnumerable<IFunction> functions)
        {
            Functions = functions;
            this.syntax = syntax;
            this.runner = runner;
        }

        public Task Run(IDictionary<string, object> dict)
        {
            return runner.Run(syntax, dict);
        }

        public IObservable<string> Messages => runner.Messages;
    }
}