using System;
using System.Threading.Tasks;
using Iridio.Binding.Model;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public interface IScriptRunner
    {
        Task<Either<RunError, ExecutionSummary>> Run(Script script);
        IObservable<string> Messages { get; }
    }
}