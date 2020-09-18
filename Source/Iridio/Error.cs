namespace Iridio
{
    public class Error
    {
        public Error()
        {
        }

        public Error(ErrorKind kind, string additionalData = null)
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

        public static implicit operator Error(ErrorKind errorKind)
        {
            return new Error(errorKind);
        }
    }
}