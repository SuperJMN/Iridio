using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Core;

namespace Iridio
{
    public interface ICompiler
    {
        Result<Script, CompilerError> Compile(string path);
    }
}