using System.Linq;
using Iridio.Parsing.Model;
using Iridio.Tokenization;
using Superpower;
using Superpower.Parsers;

namespace Iridio.Parsing
{
    public class SimpleParser
    {
        private static readonly TokenListParser<SimpleToken, string> Identifier = Token.EqualTo(SimpleToken.Identifier).Select(x => x.ToStringValue());

        private static readonly TokenListParser<SimpleToken, string> Text = Token.EqualTo(SimpleToken.Text)
            .Apply(ExtraParsers.SpanBetween('\"').Select(x => x.ToStringValue()));

        private static readonly TokenListParser<SimpleToken, int> Number = Token.EqualTo(SimpleToken.Number).Apply(Numerics.IntegerInt32);

        public static readonly TokenListParser<SimpleToken, Expression> TextParameter = Text.Select(x => (Expression)new StringExpression(x));
        public static readonly TokenListParser<SimpleToken, Expression> NumberParameter = Number.Select(x => (Expression)new NumericExpression(x));
        public static readonly TokenListParser<SimpleToken, Expression> IdentifierParameter = Identifier.Select(x => (Expression)new IdentifierExpression(x));

        private static readonly TokenListParser<SimpleToken, Expression[]> Parameters = Parse.Ref(() => Expression)
            .ManyDelimitedBy(Token.EqualTo(SimpleToken.Comma))
            .Between(SimpleToken.OpenParen, SimpleToken.CloseParen)
            .Select(objects => objects);

        public static readonly TokenListParser<SimpleToken, Expression> CallExpression = from funcName in Identifier
            from parameters in Parameters
            select (Expression)new CallExpression(funcName, parameters);

        public static readonly TokenListParser<SimpleToken, Statement>
            CallSentence = from expression in Parse.Ref(() => CallExpression)
                select (Statement) new CallStatement((CallExpression)expression);

        public static readonly TokenListParser<SimpleToken, Statement> AssignmentSentence = 
            from identifier in Identifier
            from eq in Token.EqualTo(SimpleToken.Equal)
            from expr in Expression
            select (Statement)new AssignmentStatement(identifier, expr);

        public static readonly TokenListParser<SimpleToken, Expression> Expression = CallExpression.Try().Or(NumberParameter).Or(TextParameter).Or(IdentifierParameter);

        private static readonly TokenListParser<SimpleToken, Statement> EchoSentence = Token.EqualTo(SimpleToken.Echo).Apply(ExtraParsers.SpanBetween('<', '>')).Select(span => (Statement)new EchoStatement(span.ToStringValue()) );

        public static readonly TokenListParser<SimpleToken, Statement> Sentence = from s in AssignmentSentence.Try().Or(CallSentence).Try()
            from semicolon in Token.EqualTo(SimpleToken.Semicolon)
            select s;

        public static readonly TokenListParser<SimpleToken, Statement> Statement = Sentence.Try().Or(EchoSentence);

        public static TokenListParser<SimpleToken, ScriptSyntax> SimpleScript =>
            (from statements in Statement.Many()
                select new ScriptSyntax(statements))
            .AtEnd();
    }
}