namespace SimpleScript.Parsing.Model
{
    public class BooleanOperator
    {
        public string Op { get; }

        public BooleanOperator(string op)
        {
            Op = op;
        }
    }
}