using CSharpFunctionalExtensions;
using Iridio.Core;

namespace Iridio.Common
{
    public class BinderError
    {
        public BinderError(ErrorKind kind, Maybe<Position> position, Maybe<string> additionalData)
        {
            ErrorKind = kind;
            Position = position;
            AdditionalData = additionalData;
        }

        public ErrorKind ErrorKind { get; set; }
        public Maybe<string> AdditionalData { get; set; }
        public Maybe<Position> Position { get; set; }

        public override string ToString()
        {
            return $"{nameof(ErrorKind)}: {ErrorKind}, {nameof(AdditionalData)}: {AdditionalData}";
        }

        public static implicit operator BinderError(ErrorKind errorKind)
        {
            return new BinderError(errorKind, Maybe<Position>.None, Maybe<string>.None);
        }
    }
}