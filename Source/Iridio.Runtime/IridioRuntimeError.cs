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

        public override IReadOnlyCollection<RichError> Errors => new[]
                { new RichError(Error.ToString(), Maybe<SourceUnit>.From(SourceUnit.From(Error.Position, SourceCode))) }
            .ToList()
            .AsReadOnly();
    }
}