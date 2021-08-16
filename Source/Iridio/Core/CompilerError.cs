using System.Collections.Generic;
using Iridio.Parsing;

namespace Iridio.Core
{
    public abstract class CompilerError : IErrorList
    {
        protected CompilerError(SourceCode sourceCode)
        {
            SourceCode = sourceCode;
        }

        public SourceCode SourceCode { get; }

        public abstract IReadOnlyCollection<ErrorItem> Errors { get; }
    }
}