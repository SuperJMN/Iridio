using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Core;
using Iridio.Parsing;

namespace Iridio.Runtime
{
    public class IridioRuntimeError : IridioError
    {
        public RunError Error { get; }

        public IridioRuntimeError(RunError error, SourceCode sourceCode) : base(sourceCode)
        {
            Error = error;
        }

        public override string ToString()
        {
            return $"Runtime error: {Error}";
        }

        public override IReadOnlyCollection<ErrorItem> Errors => new[]
            {
                new ErrorItem(Error.ToString(), Error.Position.Map(position => SourceUnit.From(position, SourceCode)))
            }
            .ToList()
            .AsReadOnly();
    }
}