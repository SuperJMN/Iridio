using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing;
using Iridio.Preprocessing;

namespace Iridio.Runtime
{
    // ReSharper disable once UnusedType.Global
    public class IridioShell : IIridioCore
    {
        private readonly IridioCore iridioCoreCore;
        private readonly Preprocessor preprocessor;

        public IridioShell(IList<IFunction> externalFunctions)
        {
            preprocessor = new Preprocessor(new System.IO.Abstractions.FileSystem());
            var compiler = new SourceCodeCompiler(new Binder(externalFunctions), new Parser());
            var runner = new ScriptRunner(externalFunctions);
            iridioCoreCore = new IridioCore(compiler, runner);
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(string path)
        {
            var source = preprocessor.Process(path);
            return iridioCoreCore.Run(source);
        }

        public IObservable<string> Messages => iridioCoreCore.Messages;
    }
}