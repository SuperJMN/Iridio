using System.Linq;
using Optional;
using Optional.Collections;
using Optional.Unsafe;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns;
using Expression = SimpleScript.Parsing.Model.Expression;

namespace SimpleScript.Binding
{
    public class Binder
    {
        private readonly BindingContext context;

        public Binder(BindingContext context)
        {
            this.context = context;
        }

        Either<ErrorList, BoundScript> Bind(EnhancedScript script)
        {
            var funcs = script.Functions.Select(Bind);

            return new BoundScript();
        }

        private Either<ErrorList, BoundFunction> Bind(Function func)
        {
            var function = context.Functions
                .FirstOrNone(f => f.Name == func.Name)
                .Match(f =>
                        Either.Success<ErrorList, Function>(func),
                    () => Either.Error<ErrorList, Function>(new ErrorList($"Cannot find function '{func}'")));

            var instructions = func.Statements.Select(i => Bind(i));

            return Either.Summarize(function, (Either<ErrorList, int>)1, (boundFunction, i) => (Either<ErrorList, BoundFunction>)new BoundFunction(func));
        }

        private Either<ErrorList, BoundStatement> Bind(Statement statement)
        {
            switch (statement)
            {
                case IfStatement @if:
                    return Bind(@if);
                case CallStatement call:
                    return Bind(call);
                case EchoStatement echo:
                    return Bind(echo);
                case AssignmentStatement assignment:
                    return Bind(assignment);
            }

            return new ErrorList($"Cannot bind {statement}");
        }

        private Either<ErrorList, BoundStatement> Bind(AssignmentStatement assignmentStatement)
        {
            return Either
                .Summarize(Bind(assignmentStatement.Expression), (Either<ErrorList, string>) assignmentStatement.Variable, (expression, variable) => (Either<ErrorList, BoundStatement>)new BoundAssignmentStatement(variable, expression));
        }

        private Either<ErrorList, BoundStatement> Bind(CallStatement callStatement)
        {
            throw new System.NotImplementedException();
        }

        private Either<ErrorList, BoundStatement> Bind(IfStatement ifStatement)
        {
            var cond = Bind(ifStatement.Cond);

            return new BoundIfStatement();
        }

        private Either<ErrorList, BoundCondition> Bind(Condition condition)
        {
            var left = Bind(condition.Left);
            var op = condition.Op;
            var right = Bind(condition.Right);

            return Either.Summarize(left, right, (a, b) => (Either<ErrorList, BoundCondition>) new BoundCondition(a, op, b));
        }

        private Either<ErrorList, BoundExpression> Bind(Expression expression)
        {
            switch (expression)
            {
                case CallExpression callExpression:
                    break;
                case IdentifierExpression identifierExpression:
                    break;
                case NumericExpression constant:
                    return new BoundNumericExpression(constant.Number);
                case StringExpression stringExpression:
                    break;

            }

            return new ErrorList($"Expression {expression} cannot be bound");
        }

        private Either<ErrorList, BoundStatement> Bind(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message);
        }
    }

    internal class BoundAssignmentStatement : BoundStatement
    {
        public string Variable { get; }
        public BoundExpression Expression { get; }

        public BoundAssignmentStatement(string variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }

    internal class BoundExpression
    {
    }

    internal class BoundCondition : BoundStatement
    {
        public BoundExpression Left { get; }
        public BooleanOperator Op { get; }
        public BoundExpression Right { get; }

        public BoundCondition(BoundExpression left, BooleanOperator op, BoundExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    }

    internal class BoundIfStatement : BoundStatement
    {
    }

    internal class BoundStatement
    {
    }
}