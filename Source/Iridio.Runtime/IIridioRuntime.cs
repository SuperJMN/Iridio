using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public interface IIridioRuntime
    {
        Task<Result<ExecutionSummary, RuntimeError>> Run(SourceCode sourceCode);
    }
}