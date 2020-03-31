using Superpower;
using Superpower.Parsers;

namespace ScriptParser
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
            .Between(SimpleToken.OpenParen, SimpleToken.CloseParent)
            .Select(objects => objects);

        public static readonly TokenListParser<SimpleToken, Expression> CallExpression = from id in Identifier
            from parameters in Parameters
            select (Expression)new CallExpression(parameters);

        public static readonly TokenListParser<SimpleToken, Sentence>
            CallSentence = from expression in Parse.Ref(() => Expression)
                select (Sentence) new Call(expression);

        public static readonly TokenListParser<SimpleToken, Sentence> AssignmentSentence = from s in Identifier
            from eq in Token.EqualTo(SimpleToken.Equal)
            from expr in Expression
            select (Sentence)new AssignmentSentence();

        public static readonly TokenListParser<SimpleToken, Expression> Expression = CallExpression.Try().Or(NumberParameter).Or(TextParameter).Or(IdentifierParameter);

        //private static readonly TokenListParser<SimpleToken, Sentence> EchoSentence = Token.EqualTo(SimpleToken.Echo).Apply(ExtraParsers.SpanBetween('<', '>')).Select(span => (Sentence)new EchoSentence(span.ToStringValue()) );

        public static readonly TokenListParser<SimpleToken, Sentence> Sentence = AssignmentSentence.Try().Or(CallSentence).Try();
        
        public static TokenListParser<SimpleToken, SimpleScript> SimpleScript =>
            Sentence.ManyDelimitedBy(Token.EqualTo(SimpleToken.Semicolon)).Select(x => new SimpleScript(x));
    }

    public class CallExpression : Expression
    {
        public Expression[] Parameters { get; }

        public CallExpression(Expression[] parameters)
        {
            Parameters = parameters;
        }
    }

    internal class AssignmentSentence : Sentence
    {
    }

    internal class EchoSentence : Sentence
    {
        public string Echo { get; }

        public EchoSentence(string echo)
        {
            Echo = echo;
        }
    }

    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }

        public IdentifierExpression(string identifier)
        {
            Identifier = identifier;
        }
    }

    public class NumericExpression : Expression
    {
        public NumericExpression(in int n)
        {
            Number = n;
        }

        public int Number { get; }
    }

    public class StringExpression : Expression
    {
        public string String { get; }

        public StringExpression(string str)
        {
            String = str;
        }
    }

    public abstract class Expression
    {
    }

    internal class Call : Sentence
    {
        public Expression Expression { get; }

        public Call(Expression expression)
        {
            Expression = expression;
        }
    }

    public abstract class Sentence
    {
    }

    public class SimpleScript
    {
        public Sentence[] Sentences { get; }

        public SimpleScript(Sentence[] sentences)
        {
            Sentences = sentences;
        }
    }
}