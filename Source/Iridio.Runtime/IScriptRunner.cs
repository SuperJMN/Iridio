using System.Collections.Generic;
using System.Threading.Tasks;
using Iridio.Binding.Model;
using Iridio.Common;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public interface IScriptRunner
    {
        Task<Either<Errors, Success>> Run(CompilationUnit compilationUnit, IDictionary<string, object> variables);
    }
}