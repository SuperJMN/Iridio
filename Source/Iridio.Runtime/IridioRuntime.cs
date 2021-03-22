using System.Threading.Tasks;
using Zafiro.Core.Patterns.Either;

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

        public async Task<Either<RuntimeError, ExecutionSummary>> Run(string source)
        {
            return await compiler.Compile(source)
                .MapLeft(x => (RuntimeError)new RuntimeCompileError(x))
                .MapRight(async script =>
                {
                    var runResult = await runner.Run(script);

                    return runResult
                        .MapLeft(x => (RuntimeError)new ExecutionFailed(x));
                }).RightTask();
        }
    }
}