using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Binding.Model;

namespace Iridio.Runtime
{
    public interface IInterpreter
    {
        Task<Result<ExecutionSummary, RunError>> Run(Script script);
        IObservable<string> Messages { get; }
    }
}