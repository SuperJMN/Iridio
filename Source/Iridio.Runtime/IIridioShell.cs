using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Iridio.Runtime
{
    public interface IIridioShell
    {
        Task<Result<ExecutionSummary, IridioError>> Run(string path);
        IObservable<string> Messages { get; }
    }
}