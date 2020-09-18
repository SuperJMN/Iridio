using System;

namespace Iridio.Parsing.Model
{
    public class Declaration : ISyntax
    {
        public string Key { get; }
        public string Value { get; }

        public Declaration(string identifier, string value)
        {
            Key = identifier ?? throw new ArgumentNullException(nameof(identifier));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected bool Equals(Declaration other)
        {
            return Key == other.Key && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Declaration) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Key.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{nameof(Key)}: {Key}, {nameof(Value)}: {Value}";
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}