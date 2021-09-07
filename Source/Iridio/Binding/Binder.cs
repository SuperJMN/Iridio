using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Core;
using Iridio.Parsing.Model;
using Optional.Collections;
using Zafiro.Core.Patterns;

namespace Iridio.Binding
{
    public class Binder : IBinder
    {
        private const string MainProcedureName = "Main";
        private readonly IDictionary<string, INamed> functions;
        private readonly Collection<BinderError> errors = new();

        private readonly IDictionary<string, BoundProcedure> procedures =
            new Dictionary<string, BoundProcedure>();

        private readonly ISet<string> initializedVariables = new HashSet<string>();

        public Binder(IEnumerable<INamed> externalFunctions)
        {
            if (externalFunctions == null)
            {
                throw new ArgumentNullException(nameof(externalFunctions));
            }

            functions = externalFunctions.ToDictionary(d => d.Name, d => d);
        }

        public Result<Script, BinderErrors> Bind(IridioSyntax syntax)
        {
            procedures.Clear();
            initializedVariables.Clear();
            errors.Clear();

            var boundProcedures = syntax.Procedures.Select(Bind).ToList();
            var main = boundProcedures.FirstOrNone(b => b.Name == MainProcedureName);
            main.MatchNone(() =>
                errors.Add(new BinderError(ErrorKind.UndefinedMainFunction, Maybe<Position>.None,
                    $"'{MainProcedureName}' procedure is undefined")));

            if (errors.Any())
            {
                return Result.Failure<Script, BinderErrors>(new BinderErrors(errors));
            }

            var script = new Script(boundProcedures);
            return Result.Success<Script, BinderErrors>(script);
        }

        private BoundProcedure Bind(Procedure proc)
        {
            if (functions.ContainsKey(proc.Name))
            {
                AddError(new BinderError(ErrorKind.ProcedureNameConflictsWithBuiltInFunction, proc.Position, proc.Name));
            }

            var boundProcedure = new BoundProcedure(proc.Name, new BoundBlock(proc.Block.Statements.Select(Bind).ToList(), new Position(0, 0)),
                new Position(0, 0));
            procedures.Add(proc.Name, boundProcedure);
            return boundProcedure;
        }

        private BoundStatement Bind(Statement st)
        {
            switch (st)
            {
                case AssignmentStatement assignmentStatement:
                    return Bind(assignmentStatement);
                case CallStatement callStatement:
                    return Bind(callStatement);
                case EchoStatement echoStatement:
                    return Bind(echoStatement);
                case IfStatement ifStatement:
                    return Bind(ifStatement);
                default:
                    throw new ArgumentOutOfRangeException(nameof(st));
            }
        }

        private BoundStatement Bind(AssignmentStatement assignmentStatement)
        {
            initializedVariables.Add(assignmentStatement.Target);

            return new BoundAssignmentStatement(assignmentStatement.Target, Bind(assignmentStatement.Expression), assignmentStatement.Position);
        }

        private BoundExpression Bind(Expression expression)
        {
            return expression switch
            {
                BinaryExpression binaryExpression => Bind(binaryExpression),
                BooleanValueExpression booleanValueExpression => Bind(booleanValueExpression),
                CallExpression callExpression => Bind(callExpression),
                IdentifierExpression identifierExpression => Bind(identifierExpression),
                IntegerExpression numericExpression => Bind(numericExpression),
                StringExpression stringExpression => Bind(stringExpression),
                UnaryExpression unaryExpression => Bind(unaryExpression),
                DoubleExpression doubleExpression => Bind(doubleExpression),
                _ => throw new ArgumentOutOfRangeException(nameof(expression))
            };
        }

        private BoundExpression Bind(UnaryExpression unaryExpression)
        {
            return new BoundUnaryExpression(Bind(unaryExpression.Expression), unaryExpression.Op, unaryExpression.Position);
        }

        private BoundExpression Bind(BooleanValueExpression booleanValueExpression)
        {
            return new BoundBooleanValueExpression(booleanValueExpression.Value, booleanValueExpression.Position);
        }

        private BoundExpression Bind(BinaryExpression binaryExpression)
        {
            return new BoundBinaryExpression(Bind(binaryExpression.Left), binaryExpression.Op, Bind(binaryExpression.Right),
                binaryExpression.Position);
        }

        private BoundExpression Bind(DoubleExpression doubleExpression)
        {
            return new BoundDoubleExpression(doubleExpression.Value, doubleExpression.Position);
        }

        private BoundExpression Bind(StringExpression stringExpression)
        {
            var str = stringExpression.String;
            return new BoundStringExpression(str, stringExpression.Position);
        }

        private BoundExpression Bind(IntegerExpression integerExpression)
        {
            return new BoundIntegerExpression(integerExpression.Value, integerExpression.Position);
        }

        private BoundExpression Bind(IdentifierExpression identifierExpression)
        {
            return new BoundIdentifier(identifierExpression.Identifier, identifierExpression.Position);
        }

        private void AddError(BinderError binderError)
        {
            errors.Add(binderError);
        }

        private BoundCallExpression Bind(CallExpression callExpression)
        {
            var parameters = callExpression.Parameters.Select(Bind).ToList();

            var f = functions.TryGetValue(callExpression.Name)
                .Map(f => (BoundCallExpression)new BoundFunctionCallExpression(f, parameters, callExpression.Position));

            var p = procedures.TryGetValue(callExpression.Name)
                .Map(p => (BoundCallExpression)new BoundProcedureCallExpression(p, parameters, callExpression.Position));

            var called = f.Or(p);
            called.ExecuteOnEmpty(() =>
                AddError(new BinderError(ErrorKind.UndeclaredFunctionOrProcedure, callExpression.Position, callExpression.Name)));
            return called.Unwrap(new BoundNopCallExpression(callExpression.Position));
        }

        private BoundStatement Bind(IfStatement ifStatement)
        {
            return new BoundIfStatement(Bind(ifStatement.Condition), Bind(ifStatement.TrueBlock), ifStatement.FalseBlock.Map(Bind),
                ifStatement.Position);
        }

        private BoundBlock Bind(Block block)
        {
            return new BoundBlock(block.Statements.Select(Bind).ToList(), new Position(0, 0));
        }

        private BoundStatement Bind(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message, echoStatement.Position);
        }

        private BoundStatement Bind(CallStatement callStatement)
        {
            return new BoundCallStatement(Bind(callStatement.Call), callStatement.Position);
        }
    }

    public static class FunctionalExtensions
    {
        public static Maybe<T> ExecuteOnEmpty<T>(this Maybe<T> maybe, Action action)
        {
            if (maybe.HasNoValue)
            {
                action();
            }

            return maybe;
        }
    }
}