using System;
using System.Threading.Tasks;
using Iridio.Binding.Model;
using Iridio.Runtime.ReturnValues;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public interface IScriptRunner
    {
        Task<Either<RuntimeError, ExecutionSummary>> Run(Script script);
        IObservable<string> Messages { get; }
    }
}