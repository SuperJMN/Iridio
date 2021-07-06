namespace Iridio.Parsing.Model
{
    public abstract class Operator
    {
        protected Operator(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; }
    }
}