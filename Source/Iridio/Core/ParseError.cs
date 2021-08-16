using System.Collections.Generic;
using System.Collections.ObjectModel;
using Iridio.Parsing;

namespace Iridio.Core
{
    public class ParseError : CompilerError
    {
        public ParseError(SourceUnit sourceUnit, string errorMessage, SourceCode sourceCode) : base(sourceCode)
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

        public override IReadOnlyCollection<ErrorItem> Errors => new ReadOnlyCollection<ErrorItem>(new[]
        {
            new ErrorItem(Message, SourceUnit)
        });
    }
}