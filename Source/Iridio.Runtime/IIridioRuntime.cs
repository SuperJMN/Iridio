using System.Threading.Tasks;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public interface IIridioRuntime
    {
        Task<Either<RuntimeError, ExecutionSummary>> Run(string source);
    }
}