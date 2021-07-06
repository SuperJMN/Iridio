using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Iridio.Runtime
{
    public interface IIridioRuntime
    {
        Task<Result<ExecutionSummary, RuntimeError>> Run(string source);
    }
}