using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public class IridioCore : IIridio
    {
        private readonly ISourceCodeCompiler compiler;
        private readonly IInterpreter runner;

        public IridioCore(ISourceCodeCompiler compiler, IInterpreter runner)
        {
            this.compiler = compiler;
            this.runner = runner;
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(SourceCode code)
        {
            var match = compiler
                .Compile(code)
                .MapError(compilerError => (IridioError)new IridioCompileError(compilerError, code))
                .Bind(s => runner.Run(s)
                    .MapError(error => (IridioError)new IridioRuntimeError(error, code)));

            return match;
        }

        public IObservable<string> Messages => runner.Messages;
    }
}