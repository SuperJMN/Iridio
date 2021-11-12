using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public class Iridio : IIridio
    {
        private readonly IridioCore core;

        public Iridio() : this(new HashSet<IFunction>())
        {
        }

        public Iridio(ISet<IFunction> functions)
        {
            var sourceCodeCompiler = new SourceCodeCompiler(new Binder(functions), new Parser());
            core = new IridioCore(sourceCodeCompiler, new Interpreter(functions));
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(SourceCode code, IDictionary<string, object> initialState)
        {
            return core.Run(code, initialState);
        }

        public IObservable<string> Messages => core.Messages;
    }
}