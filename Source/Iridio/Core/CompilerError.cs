using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Iridio.Core
{
    public abstract class CompilerError : IRichErrors
    {
        public abstract IReadOnlyCollection<RichError> Errors { get; }
    }

    public interface IErrors
    {
        public IReadOnlyCollection<Error> Errors { get; }
    }

    public interface IRichErrors
    {
        public IReadOnlyCollection<RichError> Errors { get; }
    }

    public class RichError
    {
        public RichError(string message, Maybe<SourceUnit> sourceUnit)
        {
            SourceUnit = sourceUnit;
            Message = message;
        }

        public Maybe<SourceUnit> SourceUnit { get; set; }
        public string Message { get; set; }
    }

    public class Error
    {
        public Error(string message, Maybe<Position> position)
        {
            Message = message;
            Position = position;
        }

        public Maybe<Position> Position { get; }

        public string Message { get; }
    }
}