namespace Iridio.Common
{
    public class BinderError
    {
        public BinderError()
        {
        }

        public BinderError(ErrorKind kind, string additionalData = null)
        {
            ErrorKind = kind;
            AdditionalData = additionalData;
        }

        public ErrorKind ErrorKind { get; set; }
        public string AdditionalData { get; set; }

        public override string ToString()
        {
            return $"{nameof(ErrorKind)}: {ErrorKind}, {nameof(AdditionalData)}: {AdditionalData}";
        }

        public static implicit operator BinderError(ErrorKind errorKind)
        {
            return new BinderError(errorKind);
        }
    }
}