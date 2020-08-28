namespace SimpleScript.Parsing.Model
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }

        public IdentifierExpression(string identifier)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return $"{Identifier}";
        }
    }
}