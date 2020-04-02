namespace SimpleScript.Ast.Model
{
    public class StringExpression : Expression
    {
        public string String { get; }

        public StringExpression(string str)
        {
            String = str;
        }
    }
}