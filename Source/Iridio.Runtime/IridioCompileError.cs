using System.Collections.Generic;
using Iridio.Core;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public class IridioCompileError : IridioError
    {
        public CompilerError CompilerError { get; }

        public IridioCompileError(CompilerError compilerError, SourceCode sourceCode) : base(sourceCode)
        {
            CompilerError = compilerError;
        }

        public override string ToString()
        {
            return $"Compiler error: {CompilerError}";
        }

        public override IReadOnlyCollection<ErrorItem> Errors => CompilerError.Errors;
    }
}