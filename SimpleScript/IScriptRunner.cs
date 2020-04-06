using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public interface IScriptRunner
    {
        Task Run(ScriptSyntax syntax, IDictionary<string, object> variables = null);
        IObservable<string> Messages { get; }
    }
}