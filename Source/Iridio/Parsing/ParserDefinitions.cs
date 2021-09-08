using CSharpFunctionalExtensions;
using Iridio.Parsing.Model;
using Iridio.Tokenization;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Position = Iridio.Core.Position;

namespace Iridio.Parsing
{
    public class ParserDefinitions
    {
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Add = Token.EqualTo(SimpleToken.Plus).Value(BinaryOperator.Add);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Subtract = Token.EqualTo(SimpleToken.Hyphen).Value(BinaryOperator.Subtract);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Multiply = Token.EqualTo(SimpleToken.Asterisk).Value(BinaryOperator.Multiply);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Divide = Token.EqualTo(SimpleToken.Slash).Value(BinaryOperator.Divide);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Lte = Token.EqualTo(SimpleToken.LessOrEqual).Value(BinaryOperator.LessThanOrEqual);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Lt = Token.EqualTo(SimpleToken.Less).Value(BinaryOperator.LessThan);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Gt = Token.EqualTo(SimpleToken.Greater).Value(BinaryOperator.GreaterThan);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Gte = Token.EqualTo(SimpleToken.GreaterOrEqual).Value(BinaryOperator.GreaterThanOrEqual);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Eq = Token.EqualTo(SimpleToken.EqualEqual).Value(BinaryOperator.Equal);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Neq = Token.EqualTo(SimpleToken.NotEqual).Value(BinaryOperator.NotEqual);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> And = Token.EqualTo(SimpleToken.And).Value(BinaryOperator.And);
        private static readonly TokenListParser<SimpleToken, BinaryOperator> Or = Token.EqualTo(SimpleToken.Or).Value(BinaryOperator.Or);
        private static readonly TokenListParser<SimpleToken, UnaryOperator> Not = Token.EqualTo(SimpleToken.Exclamation).Value(UnaryOperator.Not);

        private static readonly TokenListParser<SimpleToken, Token<SimpleToken>> Identifier =
            Token.EqualTo(SimpleToken.Identifier);

        private static string Unwrap(string str)
        {
            return str[1..^1];
        }

        public static readonly TokenListParser<SimpleToken, Expression> TextExpression =
            Token.EqualTo(SimpleToken.Text).Select(x => (Expression)new ConstantExpression(Unwrap(x.ToStringValue()), x.Position.ToPosition()));

        public static readonly TokenListParser<SimpleToken, Expression> IntegerExpression =
            Token.EqualTo(SimpleToken.Integer).Select(t =>
            {
                var i = Numerics.IntegerInt32.Parse(t.ToStringValue());
                return (Expression)new ConstantExpression(i, t.Position.ToPosition());
            });

        public static readonly TokenListParser<SimpleToken, Expression> DoubleExpression =
            Token.EqualTo(SimpleToken.Double).Select(t =>
            {
                var i = TextParsers.DoubleParser.Parse(t.ToStringValue());
                return (Expression)new ConstantExpression(i, t.Position.ToPosition());
            });

        public static readonly TokenListParser<SimpleToken, Expression> IdentifierExpression =
            Identifier.Select(x => (Expression)new IdentifierExpression(x.ToStringValue(), x.Position.ToPosition()));


        public static readonly TokenListParser<SimpleToken, Expression> BooleanValueExpression =
            Token.EqualTo(SimpleToken.True).Select(t => (Expression)new ConstantExpression(true, t.Position.ToPosition()))
                .Or(Token.EqualTo(SimpleToken.False).Select(t => (Expression)new ConstantExpression(false, t.Position.ToPosition())));

        private static readonly TokenListParser<SimpleToken, Expression[]> Parameters = Parse.Ref(() => Expression)
            .ManyDelimitedBy(Token.EqualTo(SimpleToken.Comma))
            .Between(SimpleToken.OpenParen, SimpleToken.CloseParen)
            .Select(objects => objects);

        public static readonly TokenListParser<SimpleToken, Expression> CallExpression = from funcName in Identifier
            from parameters in Parameters
            select (Expression)new CallExpression(funcName, parameters, funcName.Position.ToPosition());

        private static readonly TokenListParser<SimpleToken, Block>
            Else = from keyword in Token.EqualTo(SimpleToken.Else)
                from block in Block
                select block;

        private static readonly TokenListParser<SimpleToken, Expression> Condition =
            Parse.Ref(() => Expression)
                .Between(Token.EqualTo(SimpleToken.OpenParen), Token.EqualTo(SimpleToken.CloseParen));

        public static readonly TokenListParser<SimpleToken, Statement> IfStatement =
            from keyword in Token.EqualTo(SimpleToken.If)
            from cond in Condition
            from ifStatements in Block
            from elseStatement in Else.OptionalOrDefault()
            select (Statement)new IfStatement(cond, ifStatements, Maybe<Block>.From(elseStatement), keyword.Position.ToPosition());

        public static readonly TokenListParser<SimpleToken, Statement>
            CallStatement = from expression in Parse.Ref(() => CallExpression)
                select (Statement)new CallStatement((CallExpression)expression, expression.Position);

        public static readonly TokenListParser<SimpleToken, Statement> AssignmentSentence =
            from identifier in Identifier
            from eq in Token.EqualTo(SimpleToken.Equal)
            from expr in Expression
            select (Statement)new AssignmentStatement(identifier.ToStringValue(), identifier.Position.ToPosition(), expr);

        private static readonly TokenListParser<SimpleToken, Expression> Item = CallExpression.Try()
            .Or(IntegerExpression)
            .Or(DoubleExpression)
            .Or(TextExpression)
            .Or(IdentifierExpression)
            .Or(BooleanValueExpression);

        private static readonly TokenListParser<SimpleToken, Expression> Factor =
            Parse.Ref(() => Expression).BetweenParenthesis()
                .Or(Item);

        private static readonly TokenListParser<SimpleToken, Expression> Operand =
            (from op in Not
                from factor in Factor
                select MakeUnary(op, factor, factor.Position)).Or(Factor).Named("expression");

        private static readonly TokenListParser<SimpleToken, Expression> InnerTerm = Operand;

        private static readonly TokenListParser<SimpleToken, Expression> Term = Parse.Chain(Multiply.Or(Divide), InnerTerm,
            (@operator, expression, arg3) => MakeBinary(@operator, expression, arg3, expression.Position));

        private static readonly TokenListParser<SimpleToken, Expression> Comparand = Parse.Chain(Add.Or(Subtract), Term,
            (@operator, expression, arg3) => MakeBinary(@operator, expression, arg3, expression.Position));

        private static readonly TokenListParser<SimpleToken, Expression> Comparison = Parse.Chain(Lte.Or(Neq).Or(Lt).Or(Gte.Or(Gt)).Or(Eq), Comparand,
            (@operator, expression, arg3) => MakeBinary(@operator, expression, arg3, expression.Position));

        private static readonly TokenListParser<SimpleToken, Expression> Conjunction = Parse.Chain(And, Comparison,
            (@operator, expression, arg3) => MakeBinary(@operator, expression, arg3, expression.Position));

        private static readonly TokenListParser<SimpleToken, Expression> Disjunction = Parse.Chain(Or, Conjunction,
            (@operator, expression, arg3) => MakeBinary(@operator, expression, arg3, expression.Position));

        public static readonly TokenListParser<SimpleToken, Expression> Expression = Disjunction;

        private static readonly TokenListParser<SimpleToken, Statement> EchoStatement = Token.EqualTo(SimpleToken.Echo)
            .Select(token =>
            {
                var text = ExtraParsers.SpanBetween("'", "'").Parse(token.ToStringValue());
                return (Statement)new EchoStatement(text.ToStringValue(), token.Position.ToPosition());
            });

        public static readonly TokenListParser<SimpleToken, Statement> SingleSentence =
            from s in AssignmentSentence.Try().Or(CallStatement).Try()
            from semicolon in Token.EqualTo(SimpleToken.Semicolon)
            select s;

        public static readonly TokenListParser<SimpleToken, Statement> Sentence = IfStatement.Try().Or(SingleSentence);

        public static readonly TokenListParser<SimpleToken, Statement> Statement = Sentence.Try().Or(EchoStatement);

        public static readonly TokenListParser<SimpleToken, Block> Block =
            from statements in Statement.Many()
                .Between(Token.EqualTo(SimpleToken.OpenBrace), Token.EqualTo(SimpleToken.CloseBrace))
            select new Block(statements);

        public static TokenListParser<SimpleToken, Procedure> Function =>
            from i in Identifier
            from block in Block
            select new Procedure(i.ToStringValue(), i.Position.ToPosition(), block);

        public static TokenListParser<SimpleToken, IridioSyntax> Parser =>
            (from functions in Function.Many()
                select new IridioSyntax(functions, new Position(1, 1)))
            .AtEnd();

        private static Expression MakeBinary(BinaryOperator binaryOperatorName, Expression leftOperand, Expression rightOperand, Position position)
        {
            return new BinaryExpression(binaryOperatorName, leftOperand, rightOperand, position);
        }

        private static Expression MakeUnary(UnaryOperator binaryOperatorName, Expression factor, Position position)
        {
            return new UnaryExpression(binaryOperatorName, factor, position);
        }
    }

    public static class SuperpowerExtensions
    {
        public static Position ToPosition(this Superpower.Model.Position position)
        {
            return new Position(position.Line, position.Column);
        }
    }
}