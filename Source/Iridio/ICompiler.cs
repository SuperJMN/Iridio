using Iridio.Binding.Model;
using Iridio.Common;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    public interface ICompiler
    {
        Either<Errors, Script> Compile(string path);
    }
}