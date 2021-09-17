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
    public class IridioShell : IIridioShell
    {
        private readonly Iridio iridio;
        private readonly Preprocessor preprocessor;

        public IridioShell(IList<IFunction> externalFunctions)
        {
            preprocessor = new Preprocessor(new System.IO.Abstractions.FileSystem());
            var compiler = new SourceCodeCompiler(new Binder(externalFunctions), new Parser());
            var runner = new Interpreter(externalFunctions);
            iridio = new Iridio(compiler, runner);
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(string path)
        {
            var source = preprocessor.Process(path);
            return iridio.Run(source);
        }

        public IObservable<string> Messages => iridio.Messages;
    }
}