using System;
using System.Text.RegularExpressions;

namespace SimpleScript.Zafiro
{
    public class AppendableString
    {
        public string Value { get; }

        public AppendableString(string value)
        {
            if (!Regex.IsMatch(value, "[^\t\r\n]*"))
            {
                throw new InvalidOperationException("The input string is not supported. Only strings without tabs, carriage returns and newlines are accepted");
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator AppendableString(string str)
        {
            return new AppendableString(str);
        }
    }
}