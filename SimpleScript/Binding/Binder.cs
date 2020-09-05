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

        public Either<ErrorList, BoundScript> Bind(EnhancedScript script)
        {
            var eitherFuncs = script.Functions
                .ToObservable()
                .Select(Bind)
                .Do(func => func.WhenRight(bf => declaredFunctions.Add(bf.Name, bf)))
                .ToEnumerable();

            var eitherMain = script.Functions.FirstOrNone(f => f.Name == "Main").Match(
                _ => Either.Success<ErrorList, bool>(true), () => new ErrorList("Main function not defined"));

            var combine = CombineExtensions.Combine(eitherFuncs, Errors.Concat);
            return CombineExtensions.Combine(combine, eitherMain, (a, _) =>
            {
                var main = a.First(d => d.Name == "Main");
                return (Either<ErrorList, BoundScript>) new BoundScript(main, a);
            }, Errors.Concat);
        }

        private Either<ErrorList, BoundFunctionDeclaration> Bind(FunctionDeclaration func)
        {
            var statementsEither = CombineExtensions.Combine(func.Block.Statements.Select(Bind), (list, errorList) => Errors.Concat(errorList, errorList));
            var either = CombineExtensions.Combine<ErrorList, string, IEnumerable<BoundStatement>, BoundFunctionDeclaration>(
                Either.Success<ErrorList, string>(func.Name), statementsEither,
                (name, statements) => new BoundFunctionDeclaration(name, new BoundBlock(statements)), (list, errorList) => Errors.Concat(errorList, errorList));
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
            return CombineExtensions
                .Combine(Bind(assignmentStatement.Expression), (Either<ErrorList, string>) assignmentStatement.Variable, (expression, variable) => (Either<ErrorList, BoundStatement>)new BoundAssignmentStatement(variable, expression), (list, errorList) => Errors.Concat(errorList, errorList));
        }

        private Either<ErrorList, BoundStatement> Bind(CallStatement callStatement)
        {
            var either = Bind(callStatement.Call);
            return either.MapSuccess(expression => (BoundStatement)new BoundCallStatement((BoundCallExpression) expression));
        }

        private Either<ErrorList, BoundStatement> Bind(IfStatement ifStatement)
        {
            var cond = Bind(ifStatement.Cond);
            var trueStatements = Bind(ifStatement.IfStatements);
            var falseStatements = ifStatement.ElseStatements.Map(block => Bind(block));

            return falseStatements.Match(f => CombineExtensions.Combine(cond, trueStatements, f,
                (condition, ts, fs) => (Either<ErrorList, BoundStatement>) new BoundIfStatement(condition, ts, fs.Some()),
                (list, errorList) => Errors.Concat(errorList, errorList)), () => CombineExtensions.Combine(cond, trueStatements,
                (condition, ts) => (Either<ErrorList, BoundStatement>)new BoundIfStatement(condition, ts, Option.None<BoundBlock>()),
                (list, errorList) => Errors.Concat(errorList, errorList)));
        }

        private Either<ErrorList, BoundBlock> Bind(Block block)
        {
            var stataments = block.Statements.Select(Bind).Combine((list, errorList) => Errors.Concat(errorList, errorList));
            return stataments.MapSuccess(statements => new BoundBlock(statements));
        }

        private Either<ErrorList, BoundCondition> Bind(Condition condition)
        {
            var left = Bind(condition.Left);
            var op = condition.Op;
            var right = Bind(condition.Right);

            return CombineExtensions.Combine<ErrorList, BoundExpression, BoundExpression, BoundCondition>(left, right, (a, b) => new BoundCondition(a, op, b), (list, errorList) => Errors.Concat(errorList, errorList));
        }

        private Either<ErrorList, BoundExpression> Bind(Expression expression)
        {
            switch (expression)
            {
                case CallExpression callExpression:
                    return Bind(callExpression);
                case IdentifierExpression identifierExpression:
                    return new BoundIdentifier(identifierExpression.Identifier);
                case NumericExpression constant:
                    return new BoundNumericExpression(constant.Number);
                case StringExpression stringExpression:
                    return new BoundStringExpression(stringExpression.String);
            }

            return new ErrorList($"Expression '{expression}' could not be bound");
        }

        private Either<ErrorList, BoundExpression> Bind(CallExpression call)
        {
            var eitherParameters = call.Parameters.Select(Bind).Combine((list, errorList) => Errors.Concat(errorList, errorList));

            if (declaredFunctions.TryGetValue(call.FuncName, out var func))
            {
                return eitherParameters.MapSuccess(parameters => (BoundExpression)new BoundCustomCallExpression(func, parameters));
            }

            return context.Functions.FirstOrNone(function => function.Name == call.FuncName)
                .Match(function => eitherParameters.MapSuccess(parameters => (BoundExpression)new BoundBuiltInFunctionCallExpression(function, parameters)),
                    () => new ErrorList($"FunctionDeclaration '{call.FuncName}' isn't declared"));
        }

        private Either<ErrorList, BoundStatement> Bind(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message);
        }
    }
}