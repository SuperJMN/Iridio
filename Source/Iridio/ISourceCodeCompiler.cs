using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Core;
using Iridio.Parsing;

namespace Iridio
{
    public interface ISourceCodeCompiler
    {
        Result<Script, CompilerError> Compile(SourceCode sourceCode);
    }
}