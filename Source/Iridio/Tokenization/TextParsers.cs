using Superpower;
using Superpower.Parsers;

namespace Iridio.Tokenization
{
    public static class TextParsers
    {
        public static TextParser<double> DoubleParser
        {
            get
            {
                var decimalDouble = Numerics.DecimalDouble;
                return decimalDouble.Then(d => Character.EqualToIgnoreCase('d').Select(c => d));
            }
        }
    }
}