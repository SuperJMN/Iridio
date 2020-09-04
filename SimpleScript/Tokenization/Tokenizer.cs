using SimpleScript.Parsing;
using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace SimpleScript.Tokenization
{
    public static class Tokenizer
    {
        public static Tokenizer<SimpleToken> Create()
        {
            var builder = new TokenizerBuilder<SimpleToken>()
                .Match(ExtraParsers.SpanBetween('\"'), SimpleToken.Text)
                .Match(ExtraParsers.SpanBetween('<', '>'), SimpleToken.Echo)
                .Ignore(Span.WhiteSpace)
                .BooleanOperators()
                .Match(Character.EqualTo(':'), SimpleToken.Colon)
                .Match(Character.EqualTo('!'), SimpleToken.Exclamation)
                .Match(Character.EqualTo(','), SimpleToken.Comma)
                .Match(Character.EqualTo('='), SimpleToken.Equal)
                .Match(Character.EqualTo('('), SimpleToken.OpenParen)
                .Match(Character.EqualTo(')'), SimpleToken.CloseParent)
                .Match(Character.EqualTo('['), SimpleToken.OpenBracket)
                .Match(Character.EqualTo(']'), SimpleToken.CloseBracket)
                .Match(Character.EqualTo('{'), SimpleToken.OpenBrace)
                .Match(Character.EqualTo('}'), SimpleToken.CloseBrace)
                .Match(Character.EqualTo(';'), SimpleToken.Semicolon)
                .Match(Span.EqualTo("if"), SimpleToken.If, true)
                .Match(Span.EqualTo("else"), SimpleToken.Else, true)
                .Match(Numerics.Integer, SimpleToken.Number)
                .Match(Span.Regex(@"\w+[\d\w_]*"), SimpleToken.Identifier)
                .Build();
            return builder;
        }

        private static TokenizerBuilder<SimpleToken> BooleanOperators(this TokenizerBuilder<SimpleToken> builder)
        {
            builder
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