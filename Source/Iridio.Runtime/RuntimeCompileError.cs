using Iridio.Core;

namespace Iridio.Runtime
{
    internal class RuntimeCompileError : RuntimeError
    {
        public CompilerError CompilerError { get; }

        public RuntimeCompileError(CompilerError compilerError)
        {
            CompilerError = compilerError;
        }

        public override string ToString()
        {
            return CompilerError.ToString();
        }
    }
}