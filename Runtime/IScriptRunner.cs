using System.Collections.Generic;
using System.Threading.Tasks;
using Zafiro.Core.Patterns;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Tests
{
    public interface IScriptRunner
    {
        Task<Either<Errors, Success>> Run(string input, Dictionary<string, object> variables);
    }
}