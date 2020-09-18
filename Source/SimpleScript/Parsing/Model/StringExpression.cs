namespace SimpleScript.Parsing.Model
{
    public class StringExpression : Expression
    {
        public string String { get; }

        public StringExpression(string str)
        {
            String = str;
        }

        public override string ToString()
        {
            return $@"""{String}""";
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}