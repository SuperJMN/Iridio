using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Binding.Model;

namespace Iridio.Runtime
{
    public interface IInterpreter
    {
        Task<Result<ExecutionSummary, RunError>> Run(Script script, IDictionary<string, object> initialState);
        IObservable<string> Messages { get; }
    }
}