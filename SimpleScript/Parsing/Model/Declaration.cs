using System;

namespace SimpleScript.Parsing.Model
{
    public class Declaration : ISyntax
    {
        public string Identifier { get; }
        public string Value { get; }

        public Declaration(string identifier, string value)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected bool Equals(Declaration other)
        {
            return Identifier == other.Identifier && Value == other.Value;
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
                return (Identifier.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{nameof(Identifier)}: {Identifier}, {nameof(Value)}: {Value}";
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}