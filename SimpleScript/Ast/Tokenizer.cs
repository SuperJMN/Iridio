using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace SimpleScript.Ast
{
    public static class Tokenizer
    {
        public static Tokenizer<SimpleToken> Create()
        {
            var builder = new TokenizerBuilder<SimpleToken>()
                .Ignore(CommentParser)
                .Match(ExtraParsers.SpanBetween('\"'), SimpleToken.Text)
                .Match(ExtraParsers.SpanBetween('<', '>'), SimpleToken.Echo)
                .Ignore(Span.WhiteSpace)
                .Match(Character.EqualTo(':'), SimpleToken.Colon)
                .Match(Character.EqualTo('!'), SimpleToken.Exclamation)
                .Match(Character.EqualTo(','), SimpleToken.Comma)
                .Match(Character.EqualTo('='), SimpleToken.Equal)
                .Match(Character.EqualTo('('), SimpleToken.OpenParen)
                .Match(Character.EqualTo(')'), SimpleToken.CloseParent)
                .Match(Character.EqualTo('['), SimpleToken.OpenBracket)
                .Match(Character.EqualTo(']'), SimpleToken.CloseBracket)
                .Match(Character.EqualTo(';'), SimpleToken.Semicolon)
                .Match(Numerics.Integer, SimpleToken.Number)
                
                .Match(Span.Regex(@"\w+[\d\w_]*"), SimpleToken.Identifier)
                .Match(Span.WithoutAny(c => c == ','), SimpleToken.Value)
                .Build();
            return builder;
        }

        private static TextParser<TextSpan> CommentParser
        {
            get { return Span.EqualTo("//").IgnoreThen(Span.WithoutAny(c => c == '\n')); }
        }
    }
}