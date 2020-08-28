namespace SimpleScript.Parsing.Model
{
    internal class BooleanOperator
    {
        public string Op { get; }

        public BooleanOperator(string op)
        {
            Op = op;
        }
    }
}