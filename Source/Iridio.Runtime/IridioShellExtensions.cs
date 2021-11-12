using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public static class IridioShellExtensions 
    {
        public static Task<Result<ExecutionSummary, IridioError>> Run(this IIridioShell shell, string path)
        {
            return shell.Run(path, new Dictionary<string, object>());
        }

        public static Task<Result<ExecutionSummary, IridioError>> Run(this IIridioShell shell, SourceCode sourceCode)
        {
            return shell.Run(sourceCode, new Dictionary<string, object>());
        }
    }
}