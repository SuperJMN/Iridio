using Iridio.Parsing;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Iridio.Tokenization
{
    public static class Tokenizer
    {
        public static Tokenizer<SimpleToken> Create()
        {
            var builder = new TokenizerBuilder<SimpleToken>()
                .Match(Span.Regex(@"""((?:""""|[^""])*)"""), SimpleToken.Text)
                .Match(ExtraParsers.SpanBetween('<', '>'), SimpleToken.Echo)
                .Ignore(Span.WhiteSpace)
                .BooleanOperators()
                .Match(Character.EqualTo('-'), SimpleToken.Hyphen)
                .Match(Character.EqualTo('+'), SimpleToken.Plus)
                .Match(Character.EqualTo('/'), SimpleToken.Slash)
                .Match(Character.EqualTo('*'), SimpleToken.Asterisk)
                .Match(Character.EqualTo(':'), SimpleToken.Colon)
                .Match(Character.EqualTo('!'), SimpleToken.Exclamation)
                .Match(Character.EqualTo(','), SimpleToken.Comma)
                .Match(Character.EqualTo('='), SimpleToken.Equal)
                .Match(Character.EqualTo('('), SimpleToken.OpenParen)
                .Match(Character.EqualTo(')'), SimpleToken.CloseParen)
                .Match(Character.EqualTo('['), SimpleToken.OpenBracket)
                .Match(Character.EqualTo(']'), SimpleToken.CloseBracket)
                .Match(Character.EqualTo('{'), SimpleToken.OpenBrace)
                .Match(Character.EqualTo('}'), SimpleToken.CloseBrace)
                .Match(Character.EqualTo(';'), SimpleToken.Semicolon)
                .Match(Span.EqualTo("if"), SimpleToken.If, true)
                .Match(Span.EqualTo("else"), SimpleToken.Else, true)
                .Match(DoubleParser, SimpleToken.Double)
                .Match(Numerics.Integer, SimpleToken.Integer)
                .Match(Span.Regex(IdentifierRegex), SimpleToken.Identifier)
                .Build();
            return builder;
        }

        private static TextParser<double> DoubleParser
        {
            get
            {
                var decimalDouble = Numerics.DecimalDouble;
                return decimalDouble.Then(d => Character.EqualToIgnoreCase('d').Select(c => d));
            }
        }

        public static string IdentifierRegex => @"[A-Za-z_]+[\dA-Za-z_]*";

        private static TokenizerBuilder<SimpleToken> BooleanOperators(this TokenizerBuilder<SimpleToken> builder)
        {
            builder
                .Match(Span.EqualTo("&&"), SimpleToken.And)
                .Match(Span.EqualTo("||"), SimpleToken.Or)
                .Match(Span.EqualTo("!"), SimpleToken.Not)
                .Match(Span.EqualTo("=="), SimpleToken.EqualEqual)
                .Match(Span.EqualTo("!="), SimpleToken.NotEqual)
                .Match(Span.EqualTo(">="), SimpleToken.GreaterOrEqual)
                .Match(Span.EqualTo("<="), SimpleToken.LessOrEqual)
                .Match(Character.EqualTo('>'), SimpleToken.Greater)
                .Match(Character.EqualTo('<'), SimpleToken.Less)
                ;

            return builder;
        }
    }
}