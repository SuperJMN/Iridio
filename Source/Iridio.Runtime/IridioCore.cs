using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public class IridioCore : IIridio
    {
        private readonly ISourceCodeCompiler compiler;
        private readonly IInterpreter interpreter;

        public IridioCore(ISourceCodeCompiler compiler, IInterpreter interpreter)
        {
            this.compiler = compiler;
            this.interpreter = interpreter;
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(SourceCode code, IDictionary<string, object> initialState)
        {
            var match = compiler
                .Compile(code)
                .MapError(compilerError => (IridioError)new IridioCompileError(compilerError, code))
                .Bind(s => interpreter.Run(s, initialState)
                    .MapError(error => (IridioError)new IridioRuntimeError(error, code)));

            return match;
        }

        public IObservable<string> Messages => interpreter.Messages;
    }
}