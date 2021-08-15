using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Iridio.Core
{
    public class ParseError : CompilerError
    {
        public ParseError(SourceUnit sourceUnit, string errorMessage)
        {
            Message = errorMessage;
            SourceUnit = sourceUnit;
        }

        public SourceUnit SourceUnit { get; }

        public string Message { get; }

        public override string ToString()
        {
            return $"Syntax error at {SourceUnit}: {Message}";
        }

        public override IReadOnlyCollection<RichError> Errors => new ReadOnlyCollection<RichError>(new[]
        {
            new RichError(Message, SourceUnit)
        });
    }
}