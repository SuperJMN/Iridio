using CSharpFunctionalExtensions;

namespace Iridio.Core
{
    public class ErrorItem
    {
        public ErrorItem(string message, Maybe<SourceUnit> sourceUnit)
        {
            SourceUnit = sourceUnit;
            Message = message;
        }

        public Maybe<SourceUnit> SourceUnit { get; }
        public string Message { get; }

        public override string ToString()
        {
            return $"{SourceUnit} {Message}";
        }
    }
}