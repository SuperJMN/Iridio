using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData.Kernel;
using Optional;
using Optional.Collections;
using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Binding
{
    public class Binder : IBinder
    {
        private readonly BindingContext context;
        private readonly IDictionary<string, BoundFunctionDeclaration> declaredFunctions = new Dictionary<string, BoundFunctionDeclaration>();

        public Binder(BindingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Either<Errors, BoundScript> Bind(EnhancedScript script)
        {
            var eitherFuncs = script.Functions
                .ToObservable()
                .Select(Bind)
                .Do(func => func.WhenRight(bf => declaredFunctions.Add(bf.Name, bf)))
                .ToEnumerable();

            var eitherMain = script.Functions.FirstOrNone(f => f.Name == "Main").Match(
                _ => Either.Success<Errors, bool>(true), () => new Errors(new Error(ErrorKind.UndefinedMainFunction, "Main function not defined")));

            var combine = CombineExtensions.Combine(eitherFuncs, ErrorUtils.Concat);
            return CombineExtensions.Combine(combine, eitherMain, (a, _) =>
            {
                var main = a.First(d => d.Name == "Main");
                return (Either<Errors, BoundScript>) new BoundScript(main, a);
            }, ErrorUtils.Concat);
        }

        private Either<Errors, BoundFunctionDeclaration> Bind(FunctionDeclaration func)
        {
            var statementsEither = CombineExtensions.Combine(func.Block.Statements.Select(Bind), (list, errorList) => ErrorUtils.Concat(errorList, errorList));
            var either = CombineExtensions.Combine<Errors, string, IEnumerable<BoundStatement>, BoundFunctionDeclaration>(
                Either.Success<Errors, string>(func.Name), statementsEither,
                (name, statements) => new BoundFunctionDeclaration(name, new BoundBlock(statements)), (list, errorList) => ErrorUtils.Concat(errorList, errorList));
            return either;
        }

        private Either<Errors, BoundStatement> Bind(Statement statement)
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

            return new Errors(new Error(ErrorKind.BindError, $"Cannot bind {statement}"));
        }

        private Either<Errors, BoundStatement> Bind(AssignmentStatement assignmentStatement)
        {
            return CombineExtensions
                .Combine(Bind(assignmentStatement.Expression), (Either<Errors, string>) assignmentStatement.Variable, (expression, variable) => (Either<Errors, BoundStatement>)new BoundAssignmentStatement(variable, expression), (list, errorList) => ErrorUtils.Concat(errorList, errorList));
        }

        private Either<Errors, BoundStatement> Bind(CallStatement callStatement)
        {
            var either = Bind(callStatement.Call);
            return either.MapRight(expression => (BoundStatement)new BoundCallStatement((BoundCallExpression) expression));
        }

        private Either<Errors, BoundStatement> Bind(IfStatement ifStatement)
        {
            var cond = Bind(ifStatement.Condition);
            var trueStatements = Bind(ifStatement.TrueBlock);
            var falseStatements = ifStatement.FalseBlock.Map(block => Bind(block));

            return falseStatements.Match(f => CombineExtensions.Combine(cond, trueStatements, f,
                (condition, ts, fs) => (Either<Errors, BoundStatement>) new BoundIfStatement(condition, ts, fs.Some()),
                (list, errorList) => ErrorUtils.Concat(errorList, errorList)), () => CombineExtensions.Combine(cond, trueStatements,
                (condition, ts) => (Either<Errors, BoundStatement>)new BoundIfStatement(condition, ts, Option.None<BoundBlock>()),
                (list, errorList) => ErrorUtils.Concat(errorList, errorList)));
        }

        private Either<Errors, BoundBlock> Bind(Block block)
        {
            var stataments = block.Statements.Select(Bind).Combine((list, errorList) => ErrorUtils.Concat(errorList, errorList));
            return stataments.MapRight(statements => new BoundBlock(statements));
        }

        private Either<Errors, BoundCondition> Bind(Condition condition)
        {
            var left = Bind(condition.Left);
            var op = condition.Op;
            var right = Bind(condition.Right);

            return CombineExtensions.Combine<Errors, BoundExpression, BoundExpression, BoundCondition>(left, right, (a, b) => new BoundCondition(a, op, b), (list, errorList) => ErrorUtils.Concat(errorList, errorList));
        }

        private Either<Errors, BoundExpression> Bind(Expression expression)
        {
            switch (expression)
            {
                case CallExpression callExpression:
                    return Bind(callExpression);
                case IdentifierExpression identifierExpression:
                    return new BoundIdentifier(identifierExpression.Identifier);
                case NumericExpression constant:
                    return new BoundNumericExpression(constant.Value);
                case StringExpression stringExpression:
                    return new BoundStringExpression(stringExpression.String);
            }

            return new Errors(new Error(ErrorKind.BindError, $"Expression '{expression}' could not be bound"));
        }

        private Either<Errors, BoundExpression> Bind(CallExpression call)
        {
            var eitherParameters = call.Parameters.Select(Bind).Combine((list, errorList) => ErrorUtils.Concat(errorList, errorList));

            if (declaredFunctions.TryGetValue(call.Name, out var func))
            {
                return eitherParameters.MapRight(parameters => (BoundExpression)new BoundCustomCallExpression(func, parameters));
            }

            return context.Functions.FirstOrNone(function => function.Name == call.Name)
                .Match(function => eitherParameters.MapRight(parameters => (BoundExpression)new BoundBuiltInFunctionCallExpression(function, parameters)),
                    () => new Errors(new Error(ErrorKind.UndeclaredFunction, $"FunctionDeclaration '{call.Name}' isn't declared")));
        }

        private Either<Errors, BoundStatement> Bind(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message);
        }
    }
}