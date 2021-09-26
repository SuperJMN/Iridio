using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Iridio.Runtime
{
    public interface IIridioShell
    {
        Task<Result<ExecutionSummary, IridioError>> Run(string path, IDictionary<string, object> initialState);
        IObservable<string> Messages { get; }
    }
}