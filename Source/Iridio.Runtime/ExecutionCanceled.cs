using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Core;
using Iridio.Parsing;
using Iridio.Preprocessing;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace Iridio.Runtime
{
    public class ExecutionCanceled : RunError
    {
        public string Message { get; }

        public ExecutionCanceled(string message)
        {
            Message = message;
        }

        public override IEnumerable<string> Items => new[] {ToString()};

        public override string ToString()
        {
            return "Execution canceled";
        }
    }


    public interface IIridioCore
    {
        Task<Result<ExecutionSummary, IridioError>> Run(string path);
        IObservable<string> Messages { get; }
    }

    public class IridioShell : IIridioCore
    {
        private readonly IPathBasedCompiler compiler;
        private readonly ScriptRunner runner;
        private readonly IridioCore iridioCore;

        public IridioShell(IList<IFunction> externalFunctions, IFileSystem fileSystem)
        {
            compiler = new Compiler(new Preprocessor(fileSystem), new SourceCodeCompiler(new Binder(externalFunctions), new Parser()));
            runner = new ScriptRunner(externalFunctions);
            iridioCore = new IridioCore(compiler, runner);
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(string path)
        {
            return iridioCore.Run(path);
        }

        public IObservable<string> Messages => iridioCore.Messages;
    }

    internal class IridioCore : IIridioCore
    {
        private readonly IPathBasedCompiler compiler;
        private readonly IScriptRunner runner;

        public IridioCore(IPathBasedCompiler compiler, IScriptRunner runner)
        {
            this.compiler = compiler;
            this.runner = runner;
        }

        public IObservable<string> Messages => runner.Messages;

        public Task<Result<ExecutionSummary, IridioError>> Run(string path)
        {
            var match = compiler
                .Compile(path)
                .MapError(compilerError => (IridioError)new IridioCompileError(compilerError))
                .Bind(s => runner.Run(s)
                    .MapError(error => (IridioError)new IridioRuntimeError(error)));


            return match;
        }
    }

    public abstract class IridioError
    {
    }

    public class IridioCompileError : IridioError
    {
        public CompilerError CompilerError { get; }

        public IridioCompileError(CompilerError compilerError)
        {
            CompilerError = compilerError;
        }
    }

    public class IridioRuntimeError : IridioError
    {
        public RunError Error { get; }

        public IridioRuntimeError(RunError error)
        {
            Error = error;
        }
    }
}