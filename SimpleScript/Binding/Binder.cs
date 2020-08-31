using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData.Kernel;
using MoreLinq.Extensions;
using Optional.Collections;
using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns;

namespace SimpleScript.Binding
{
    public class Binder
    {
        private readonly BindingContext context;

        /// <summary>
        /// Delete this
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="eithers"></param>
        /// <param name="combineError"></param>
        /// <returns></returns>
        public static Either<TLeft, IEnumerable<TResult>> Combine<TLeft, TResult>(IEnumerable<Either<TLeft, TResult>> eithers, Func<TLeft, TLeft, TLeft> combineError)
        {
            var errors = eithers.Partition(x => x.IsRight);

            if (errors.False.Any())
            {
                var aggregate = errors.False
                    .SelectMany(either => either.LeftValue.ToEnumerable())
                    .Aggregate(combineError);

                return Either.Error<TLeft, IEnumerable<TResult>>(aggregate);
            }

            var p = errors.True.SelectMany(either => either.RightValue.ToEnumerable());
            return Either.Success<TLeft, IEnumerable<TResult>>(p);
        }

        public Binder(BindingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Either<ErrorList, BoundScript> Bind(EnhancedScript script)
        {
            var funcs = script.Functions.Select(Bind);

            return Either.Combine(funcs, functions => (Either<ErrorList, BoundScript>) new BoundScript(functions));
        }

        private Either<ErrorList, BoundFunction> Bind(Function func)
        {
            var statementsEither = Combine(func.Statements.Select(Bind), MergeErrors);
            var either = Either.Combine<ErrorList, string, IEnumerable<BoundStatement>, BoundFunction>(
                Either.Success<ErrorList, string>(func.Name), statementsEither,
                (name, statements) => new BoundFunction(name, new BoundBlock(statements)), MergeErrors);
            return either;
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
                .Combine(Bind(assignmentStatement.Expression), (Either<ErrorList, string>) assignmentStatement.Variable, (expression, variable) => (Either<ErrorList, BoundStatement>)new BoundAssignmentStatement(variable, expression), MergeErrors);
        }

        private Either<ErrorList, BoundStatement> Bind(CallStatement callStatement)
        {
            throw new System.NotImplementedException();
        }

        private Either<ErrorList, BoundStatement> Bind(IfStatement ifStatement)
        {
            var cond = Bind(ifStatement.Cond);
            var trueStatements = Bind(ifStatement.IfStatements);
            var falseStatements = Bind(ifStatement.ElseStatements);

            return Either.Combine(cond, trueStatements, falseStatements,
                (condition, ts, fs) => (Either<ErrorList, BoundStatement>)new BoundIfStatement(condition, new BoundBlock(ts), new BoundBlock(fs)),
                MergeErrors);
        }

        private Either<ErrorList, IEnumerable<BoundStatement>> Bind(Statement[] statements)
        {
            return Combine(statements.Select(Bind), MergeErrors);
        }

        private Either<ErrorList, BoundCondition> Bind(Condition condition)
        {
            var left = Bind(condition.Left);
            var op = condition.Op;
            var right = Bind(condition.Right);

            return Either.Combine<ErrorList, BoundExpression, BoundExpression, BoundCondition>(left, right, (a, b) => new BoundCondition(a, op, b), MergeErrors);
        }

        private static ErrorList MergeErrors(ErrorList list, ErrorList errorList)
        {
            return new ErrorList(list.Concat(errorList));
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
}