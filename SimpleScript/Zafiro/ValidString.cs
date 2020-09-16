using System;
using System.Text.RegularExpressions;

namespace SimpleScript.Zafiro
{
    public abstract class ValidString
    {
        public string Value { get; }

        public ValidString(string value)
        {
            if (Regex.IsMatch(value, IsValidRegex))
            {
                throw new InvalidOperationException(ValidationMessage);
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        protected abstract string IsValidRegex { get; }

        protected virtual string ValidationMessage =>
            $"The string isn't valid. It should conform to the regular expression defined by {IsValidRegex}";
    }
}