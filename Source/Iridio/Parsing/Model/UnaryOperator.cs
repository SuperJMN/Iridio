using System;

namespace Iridio.Parsing.Model
{
    public class UnaryOperator : Operator
    {
        public static UnaryOperator Not = new("!", (a) => !a);

        public Func<dynamic, object> Calculate { get; }

        public UnaryOperator(string symbol, Func<dynamic, object> calculate) : base(symbol)
        {
            Calculate = calculate;
        }
    }
}