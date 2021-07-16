using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Core;

namespace Iridio
{
    public interface IPathBasedCompiler
    {
        Result<Script, CompilerError> Compile(string path);
    }
}