using System.Linq;
using Optional;
using SimpleScript.Parsing.Model;
using SimpleScript.Tokenization;
using Superpower;
using Superpower.Parsers;

namespace SimpleScript.Parsing
{
    public class EnhancedParsers
    {
        private static readonly TokenListParser<SimpleToken, string> Identifier =
            Token.EqualTo(SimpleToken.Identifier).Select(x => x.ToStringValue());

        private static readonly TokenListParser<SimpleToken, string> Text = Token.EqualTo(SimpleToken.Text)
            .Apply(ExtraParsers.SpanBetween('\"').Select(x => x.ToStringValue()));

        private static readonly TokenListParser<SimpleToken, int> Number =
            Token.EqualTo(SimpleToken.Number).Apply(Numerics.IntegerInt32);

        public static readonly TokenListParser<SimpleToken, Expression> TextParameter =
            Text.Select(x => (Expression) new StringExpression(x));

        public static readonly TokenListParser<SimpleToken, Expression> NumberParameter =
            Number.Select(x => (Expression) new NumericExpression(x));

        public static readonly TokenListParser<SimpleToken, Expression> IdentifierParameter =
            Identifier.Select(x => (Expression) new IdentifierExpression(x));

        private static readonly TokenListParser<SimpleToken, Declaration> Declaration =
            (from id in Identifier
                from colon in Token.EqualTo(SimpleToken.Colon)
                from text in Identifier
                select new {id, text})
            .Between(Token.EqualTo(SimpleToken.OpenBracket), Token.EqualTo(SimpleToken.CloseBracket))
            .Select(arg => new Declaration(arg.id, arg.text));

        private static readonly TokenListParser<SimpleToken, Header> Header =
            Declaration.Many().Select(declarations => new Header(declarations));

        private static readonly TokenListParser<SimpleToken, Expression[]> Parameters = Parse.Ref(() => Expression)
            .ManyDelimitedBy(Token.EqualTo(SimpleToken.Comma))
            .Between(SimpleToken.OpenParen, SimpleToken.CloseParent)
            .Select(objects => objects);

        public static readonly TokenListParser<SimpleToken, Expression> CallExpression = from funcName in Identifier
            from parameters in Parameters
            select (Expression) new CallExpression(funcName, parameters);

        private static readonly TokenListParser<SimpleToken, Block>
            Else = from keyword in Token.EqualTo(SimpleToken.Else)
                from block in Block
                select block;

        private static readonly TokenListParser<SimpleToken, BooleanOperator> BooleanOperators =
            Token.EqualTo(SimpleToken.EqualEqual)
                .Or(Token.EqualTo(SimpleToken.NotEqual))
                .Or(Token.EqualTo(SimpleToken.Greater))
                .Or(Token.EqualTo(SimpleToken.Less))
                .Or(Token.EqualTo(SimpleToken.LessOrEqual))
                .Or(Token.EqualTo(SimpleToken.GreaterOrEqual))
                .Select(token => new BooleanOperator(token.ToStringValue()));

        private static readonly TokenListParser<SimpleToken, Condition> Condition =
            (from left in Parse.Ref(() => Expression)
                from op in BooleanOperators
                from right in Expression
                select new Condition(left, op, right))
            .Between(Token.EqualTo(SimpleToken.OpenParen), Token.EqualTo(SimpleToken.CloseParent));

        public static readonly TokenListParser<SimpleToken, Statement> IfStatement =
            from keyword in Token.EqualTo(SimpleToken.If)
            from cond in Condition
            from ifStatements in Block
            from elseStatement in Else.OptionalOrDefault()
            select (Statement) new IfStatement(cond, ifStatements, elseStatement.SomeNotNull());

        public static readonly TokenListParser<SimpleToken, Statement>
            CallSentence = from expression in Parse.Ref(() => CallExpression)
                select (Statement) new CallStatement((CallExpression) expression);

        public static readonly TokenListParser<SimpleToken, Statement> AssignmentSentence =
            from identifier in Identifier
            from eq in Token.EqualTo(SimpleToken.Equal)
            from expr in Expression
            select (Statement) new AssignmentStatement(identifier, expr);

        public static readonly TokenListParser<SimpleToken, Statement> ScriptCallSentence =
            from eq in Token.EqualTo(SimpleToken.Exclamation)
            from path in Text
            select (Statement) new ScriptCallStatement(path);

        public static readonly TokenListParser<SimpleToken, Expression> Expression =
            CallExpression.Try().Or(NumberParameter).Or(TextParameter).Or(IdentifierParameter);

        private static readonly TokenListParser<SimpleToken, Statement> EchoSentence = Token.EqualTo(SimpleToken.Echo)
            .Apply(ExtraParsers.SpanBetween('<', '>'))
            .Select(span => (Statement) new EchoStatement(span.ToStringValue()));

        public static readonly TokenListParser<SimpleToken, Statement> SingleSentence =
            from s in AssignmentSentence.Try().Or(CallSentence).Try().Or(ScriptCallSentence)
            from semicolon in Token.EqualTo(SimpleToken.Semicolon)
            select s;

        public static readonly TokenListParser<SimpleToken, Statement> Sentence = SingleSentence.Try().Or(IfStatement);

        public static readonly TokenListParser<SimpleToken, Statement> Statement = Sentence.Try().Or(EchoSentence);

        public static readonly TokenListParser<SimpleToken, Block> Block =
            from statements in Statement.Many()
                .Between(Token.EqualTo(SimpleToken.OpenBrace), Token.EqualTo(SimpleToken.CloseBrace))
            select new Block(statements);

        public static TokenListParser<SimpleToken, FunctionDeclaration> Function => 
            from i in Identifier
            from block in Block
            select new FunctionDeclaration(i, block);

        public static TokenListParser<SimpleToken, EnhancedScript> Parser =>
            (from header in Header.Try().OptionalOrDefault()
                from functions in Function.Many()
                select new EnhancedScript(header ?? new Header(Enumerable.Empty<Declaration>()), functions))
            .AtEnd();
    }
}