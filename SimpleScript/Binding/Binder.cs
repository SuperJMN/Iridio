using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MoreLinq;
using Optional;
using Optional.Collections;
using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Binding
{
    public class Binder : IBinder
    {
        private readonly BindingContext context;
        private readonly IDictionary<string, BoundFunctionDeclaration> declaredFunctions = new Dictionary<string, BoundFunctionDeclaration>();
        private readonly ISet<string> initializedVariables = new HashSet<string>();

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

            var declarations = Bind(script.Header);

            var eitherMain = script.Functions.FirstOrNone(f => f.Name == "Main").Match(
                _ => Either.Success<Errors, bool>(true), () => new Errors(new Error(ErrorKind.UndefinedMainFunction, "Main function not defined")));

            var combine = CombineExtensions.Combine(eitherFuncs, Errors.Concat);
            return CombineExtensions.Combine(combine, eitherMain, (a, _) =>
            {
                var main = a.First(d => d.Name == "Main");
                return (Either<Errors, BoundScript>) new BoundScript(main, a);
            }, Errors.Concat);
        }

        private BoundHeader Bind(Header header)
        {
            return new BoundHeader(header.Declarations.Select(Bind));
        }

        private BoundDeclaration Bind(Declaration decl)
        {
            return new BoundDeclaration(decl.Key, decl.Value);
        }

        private Either<Errors, BoundFunctionDeclaration> Bind(FunctionDeclaration func)
        {
            var statementsEither = CombineExtensions.Combine(func.Block.Statements.Select(Bind), (list, errorList) => Errors.Concat(errorList, errorList));
            var either = CombineExtensions.Combine<Errors, string, IEnumerable<BoundStatement>, BoundFunctionDeclaration>(
                Either.Success<Errors, string>(func.Name), statementsEither,
                (name, statements) => new BoundFunctionDeclaration(name, new BoundBlock(statements)), (list, errorList) => Errors.Concat(errorList, errorList));
            return either;
        }

        private Either<Errors, BoundStatement> Bind(Statement statement)
        {
            switch (statement)
            {
                case IfStatement ifs:
                    return Bind(ifs);
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
                .Combine(Bind(assignmentStatement.Expression), (Either<Errors, string>) assignmentStatement.Variable, (expression, variable) =>
                {
                    var assignment = new BoundAssignmentStatement(variable, expression);
                    initializedVariables.Add(variable);
                    return (Either<Errors, BoundStatement>) assignment;
                }, (list, errorList) => Errors.Concat(errorList, errorList));
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
                (list, errorList) => Errors.Concat(errorList, errorList)), () => CombineExtensions.Combine(cond, trueStatements,
                (condition, ts) => (Either<Errors, BoundStatement>)new BoundIfStatement(condition, ts, Option.None<BoundBlock>()),
                (list, errorList) => Errors.Concat(errorList, errorList)));
        }

        private Either<Errors, BoundBlock> Bind(Block block)
        {
            var stataments = block.Statements.Select(Bind).Combine((list, errorList) => Errors.Concat(errorList, errorList));
            return stataments.MapRight(statements => new BoundBlock(statements));
        }

        private Either<Errors, BoundCondition> Bind(Condition condition)
        {
            var left = Bind(condition.Left);
            var op = condition.Op;
            var right = Bind(condition.Right);

            return CombineExtensions.Combine<Errors, BoundExpression, BoundExpression, BoundCondition>(left, right,
                (a, b) => new BoundCondition(a, op, b), Errors.Concat);
        }

        private Either<Errors, BoundExpression> Bind(Expression expression)
        {
            switch (expression)
            {
                case CallExpression callExpression:
                    return Bind(callExpression);
                case IdentifierExpression identifierExpression:
                    return Bind(identifierExpression);
                case NumericExpression constant:
                    return new BoundNumericExpression(constant.Value);
                case StringExpression stringExpression:
                    return Bind(stringExpression);
            }

            return new Errors(new Error(ErrorKind.BindError, $"Expression '{expression}' could not be bound"));
        }

        private Either<Errors, BoundExpression> Bind(IdentifierExpression identifierExpression)
        {
            if (!initializedVariables.Contains(identifierExpression.Identifier))
            {
                return new Errors(new Error(ErrorKind.ReferenceToUninitializedVariable, identifierExpression.Identifier));
            }

            return new BoundIdentifier(identifierExpression.Identifier);
        }

        private Either<Errors, BoundExpression> Bind(StringExpression stringExpression)
        {
            var references = References.FromString(stringExpression.String);
            var variableIsInitialized = references.Partition(s => initializedVariables.Contains(s));
            if (variableIsInitialized.False.Any())
            {
                return new Errors(variableIsInitialized.False.Select(variable => new Error(ErrorKind.ReferenceToUninitializedVariable, variable)));
            }
            
            return new BoundStringExpression(stringExpression.String);
        }

        private Either<Errors, BoundExpression> Bind(CallExpression call)
        {
            var eitherParameters = call.Parameters.Select(Bind).Combine((list, errorList) => Errors.Concat(errorList, errorList));

            if (declaredFunctions.TryGetValue(call.Name, out var func))
            {
                return eitherParameters.MapRight(parameters => (BoundExpression)new BoundCustomCallExpression(func, parameters));
            }

            return context.Functions.FirstOrNone(function => function.Name == call.Name)
                .Match(function => eitherParameters.MapRight(parameters =>
                    {
                        return (BoundExpression) new BoundBuiltInFunctionCallExpression(function, parameters);
                    }),
                    () => new Errors(new Error(ErrorKind.UndeclaredFunction, $"FunctionDeclaration '{call.Name}' isn't declared")));
        }

        private Either<Errors, BoundStatement> Bind(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message);
        }
    }
}