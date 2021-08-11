using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Iridio.Runtime
{
    public interface IIridioCore
    {
        Task<Result<ExecutionSummary, IridioError>> Run(string path);
        IObservable<string> Messages { get; }
    }
}