namespace SimpleScript.Parsing.Model
{
    public class CallStatement : Statement
    {
        public CallExpression Call { get; }

        public CallStatement(CallExpression call)
        {
            Call = call;
        }

        public override string ToString()
        {
            return $"Call: {Call}";
        }
    }
}