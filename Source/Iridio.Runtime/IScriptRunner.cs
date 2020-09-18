using System.Collections.Generic;
using System.Threading.Tasks;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public interface IScriptRunner
    {
        Task<Either<Errors, Success>> Run(string input, Dictionary<string, object> variables);
    }
}