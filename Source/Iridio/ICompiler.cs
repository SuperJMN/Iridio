using CSharpFunctionalExtensions;
using Iridio.Binding.Model;

namespace Iridio
{
    public interface ICompiler
    {
        Result<Script, CompilerError> Compile(string path);
    }
}