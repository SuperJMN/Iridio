namespace SimpleScript
{
    public class Error
    {
        public Error(ErrorKind kind, string additionalData = null)
        {
            ErrorKind = kind;
            AdditionalData = additionalData;
        }

        public ErrorKind ErrorKind { get; }
        public string AdditionalData { get; }

        public override string ToString()
        {
            return $"{nameof(ErrorKind)}: {ErrorKind}, {nameof(AdditionalData)}: {AdditionalData}";
        }

        public static implicit operator Error(ErrorKind errorKind)
        {
            return new Error(errorKind);
        }
    }
}