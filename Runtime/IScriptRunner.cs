using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleScript;
using SimpleScript.Tests;
using Zafiro.Core.Patterns.Either;

namespace Runtime
{
    public interface IScriptRunner
    {
        Task<Either<Errors, Success>> Run(string input, Dictionary<string, object> variables);
    }
}