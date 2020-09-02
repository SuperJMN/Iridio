using System.Collections.Generic;
using System.Threading.Tasks;
using Zafiro.Core.Patterns;

namespace SimpleScript.Tests
{
    internal interface IScriptRunner
    {
        Task<Either<ErrorList, Success>> Run(string input, Dictionary<string, object> variables);
    }
}