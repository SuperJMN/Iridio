using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Iridio.Parsing;

namespace Iridio.Core
{
    public abstract class CompilerError : IRichErrors
    {
        protected CompilerError(SourceCode sourceCode)
        {
            SourceCode = sourceCode;
        }

        public SourceCode SourceCode { get; }

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

        public Maybe<SourceUnit> SourceUnit { get; }
        public string Message { get; }
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