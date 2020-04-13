namespace SimpleScript.Ast.Model
{
    public class NumericExpression : Expression
    {
        public NumericExpression(in int n)
        {
            Number = n;
        }

        public int Number { get; }

        public override string ToString()
        {
            return $"{Number}";
        }
    }
}