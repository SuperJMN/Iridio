using Iridio.Binding.Model;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    public interface ICompiler
    {
        Either<CompilerError, Script> Compile(string path);
    }
}