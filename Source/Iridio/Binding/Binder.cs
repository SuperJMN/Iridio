using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing.Model;
using MoreLinq;
using Optional;
using Optional.Collections;
using Zafiro.Core.Mixins;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Binding
{
    public class Binder : IBinder
    {
        private readonly BindingContext context;
        private readonly IDictionary<string, BoundFunctionDeclaration> declaredProcedures = new Dictionary<string, BoundFunctionDeclaration>();
        private readonly ISet<string> initializedVariables = new HashSet<string>();

        public Binder(BindingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Either<Errors, CompilationUnit> Bind(EnhancedScript script)
        {
            declaredProcedures.Clear();

            var procedures = BindProcedures(script.Procedures);
            var header = Bind(script.Header);

            var eitherMain = script.Procedures.FirstOrNone(f => f.Name == "Main").Match(
                _ => Either.Success<Errors, bool>(true), () => new Errors(new Error(ErrorKind.UndefinedMainFunction, "Main procedure not defined")));

            return CombineExtensions.Combine(procedures, eitherMain, (functions, _) =>
            {
                var main = functions.First(d => d.Name == "Main");
                return (Either<Errors, CompilationUnit>)new CompilationUnit(main, header, functions);
            }, Errors.Concat);
        }

        private Either<Errors, IEnumerable<BoundFunctionDeclaration>> BindProcedures(IEnumerable<ProcedureDeclaration> procs)
        {
            var eitherFuncs = procs
                .Select(decl => new { Bound = Bind(decl), Decl = decl })
                .Select(arg => arg.Bound);

            var combine = CombineExtensions.Combine(eitherFuncs, Errors.Concat);
            return combine;
        }

        private BoundHeader Bind(Header header)
        {
            return new BoundHeader(header.Declarations.Select(Bind));
        }

        private static BoundDeclaration Bind(Declaration decl)
        {
            return new BoundDeclaration(decl.Key, decl.Value);
        }

        private Either<Errors, BoundFunctionDeclaration> Bind(ProcedureDeclaration func)
        {
            if (declaredProcedures.ContainsKey(func.Name))
            {
                return new Errors(new Error(ErrorKind.ProcedureAlreadyDeclared, func.Name));
            }

            var statementsEither = CombineExtensions.Combine(func.Block.Statements.Select(Bind), Errors.Concat);
            var either = CombineExtensions.Combine<Errors, string, IEnumerable<BoundStatement>, BoundFunctionDeclaration>(
                Either.Success<Errors, string>(func.Name), statementsEither,
                (name, statements) => new BoundFunctionDeclaration(name, new BoundBlock(statements)), Errors.Concat);
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
                .Combine(Bind(assignmentStatement.Expression), (Either<Errors, string>)assignmentStatement.Variable, (expression, variable) =>
               {
                   var assignment = new BoundAssignmentStatement(variable, expression);
                   initializedVariables.Add(variable);
                   return (Either<Errors, BoundStatement>)assignment;
               }, Errors.Concat);
        }

        private Either<Errors, BoundStatement> Bind(CallStatement callStatement)
        {
            var either = Bind(callStatement.Call);
            return either.MapRight(expression => (BoundStatement)new BoundCallStatement((BoundCallExpression)expression));
        }

        private Either<Errors, BoundStatement> Bind(IfStatement ifStatement)
        {
            var cond = Bind(ifStatement.Condition);
            var trueStatements = Bind(ifStatement.TrueBlock);
            var falseStatements = ifStatement.FalseBlock.Map(block => Bind(block));

            return falseStatements.Match(f => CombineExtensions.Combine(cond, trueStatements, f,
                (condition, ts, fs) => (Either<Errors, BoundStatement>)new BoundIfStatement(condition, ts, fs.Some()),
                Errors.Concat), () => CombineExtensions.Combine(cond, trueStatements,
                (condition, ts) => (Either<Errors, BoundStatement>)new BoundIfStatement(condition, ts, Option.None<BoundBlock>()),
                Errors.Concat));
        }

        private Either<Errors, BoundBlock> Bind(Block block)
        {
            var stataments = block.Statements.Select(Bind).Combine(Errors.Concat);
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
            var eitherParameters = call.Parameters.Select(Bind).Combine(Errors.Concat);

            return declaredProcedures.GetValueOrNone(call.Name)
                .Match(
                    func => eitherParameters.MapRight(parameters =>
                        (BoundExpression)new BoundProcedureCallExpression(func, parameters)),
                    () =>
                    {
                        return context.Functions.FirstOrNone(function => function.Name == call.Name)
                            .Match(function => eitherParameters.MapRight(parameters => (BoundExpression)new BoundBuiltInFunctionCallExpression(function, parameters)),
                                () => new Errors(new Error(ErrorKind.UndeclaredFunctionOrProcedure, $"Undeclared function or procedure '{call.Name}'")));
                    });
        }

        private Either<Errors, BoundStatement> Bind(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message);
        }
    }
}