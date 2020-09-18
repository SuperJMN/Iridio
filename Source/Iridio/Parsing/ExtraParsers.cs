using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Iridio.Parsing
{
    public static class ExtraParsers
    {
        public static TextParser<TextSpan> SpanBetween(char left, char? right = null)
        {
            var rightDelimiter = right ?? left;
            return Span
                .MatchedBy(Character.Except(rightDelimiter).Many())
                .Between(Character.EqualTo(left), Character.EqualTo(rightDelimiter));

        }

        public static TokenListParser<TToken, T> Between<TToken, T>(this TokenListParser<TToken, T> self, TToken left, TToken right)
        {
            return self.Between(Token.EqualTo(left), Token.EqualTo(right));
        }
    }
}