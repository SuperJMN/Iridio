using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public class IridioRuntime : IIridioRuntime
    {
        private readonly ICompiler compiler;
        private readonly IScriptRunner runner;

        public IridioRuntime(ICompiler compiler, IScriptRunner runner)
        {
            this.compiler = compiler;
            this.runner = runner;
        }

        public async Task<Result<ExecutionSummary, RuntimeError>> Run(SourceCode sourceCode)
        {
            var result = await compiler.Compile(sourceCode)
                .MapError(x => (RuntimeError) new RuntimeCompileError(x))
                .Bind(async script =>
                {
                    var runResult = await runner.Run(script);

                    return runResult
                        .MapError(x => (RuntimeError) new ExecutionFailed(x));
                });

            return result;
        }
    }
}