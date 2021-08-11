using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public interface IIridio
    {
        Task<Result<ExecutionSummary, IridioError>> Run(SourceCode code);
        IObservable<string> Messages { get; }
    }
}