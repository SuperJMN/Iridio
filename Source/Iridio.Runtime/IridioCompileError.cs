using Iridio.Core;

namespace Iridio.Runtime
{
    public class IridioCompileError : IridioError
    {
        public CompilerError CompilerError { get; }

        public IridioCompileError(CompilerError compilerError)
        {
            CompilerError = compilerError;
        }
    }
}