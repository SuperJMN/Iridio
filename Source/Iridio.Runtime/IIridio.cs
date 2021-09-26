using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public interface IIridio
    {
        Task<Result<ExecutionSummary, IridioError>> Run(SourceCode code, IDictionary<string, object> initialState);
        IObservable<string> Messages { get; }
    }
}