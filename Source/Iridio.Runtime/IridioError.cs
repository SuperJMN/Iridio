using System.Collections.Generic;
using Iridio.Core;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public abstract class IridioError : IErrorList
    {
        public SourceCode SourceCode { get; }

        protected IridioError(SourceCode sourceCode)
        {
            SourceCode = sourceCode;
        }

        public abstract IReadOnlyCollection<ErrorItem> Errors { get; }
    }
}