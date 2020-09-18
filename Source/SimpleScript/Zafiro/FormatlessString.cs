namespace SimpleScript.Zafiro
{
    public class FormatlessString : ValidString
    {
        private FormatlessString(string value) : base(value)
        {
        }
        
        public static implicit operator FormatlessString(string str)
        {
            return new FormatlessString(str);
        }

        protected override string IsValidRegex => "[\t\r\n]";

        protected override string ValidationMessage =>
            "The input string is not supported. Only strings without tabs, carriage returns and newlines are accepted";
    }
}