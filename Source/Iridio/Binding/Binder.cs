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

            var boundProcedure = new BoundProcedure(proc.Name, new BoundBlock(proc.Block.Statements.Select(Bind).ToList()));
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

            return new BoundAssignmentStatement(assignmentStatement.Target, Bind(assignmentStatement.Expression));
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
            return new BoundUnaryExpression(Bind(unaryExpression.Expression), unaryExpression.Op);
        }

        private BoundExpression Bind(BooleanValueExpression booleanValueExpression)
        {
            return new BoundBooleanValueExpression(booleanValueExpression.Value);
        }

        private BoundExpression Bind(BinaryExpression binaryExpression)
        {
            return new BoundBinaryExpression(Bind(binaryExpression.Left), binaryExpression.Op, Bind(binaryExpression.Right));
        }

        private BoundExpression Bind(DoubleExpression doubleExpression)
        {
            return new BoundDoubleExpression(doubleExpression.Value);
        }

        private BoundExpression Bind(StringExpression stringExpression)
        {
            var str = stringExpression.String;
            return new BoundStringExpression(str);
        }

        private BoundExpression Bind(IntegerExpression integerExpression)
        {
            return new BoundIntegerExpression(integerExpression.Value);
        }

        private BoundExpression Bind(IdentifierExpression identifierExpression)
        {
            return new BoundIdentifier(identifierExpression.Identifier);
        }

        private void AddError(BinderError binderError)
        {
            errors.Add(binderError);
        }

        private BoundCallExpression Bind(CallExpression callExpression)
        {
            var parameters = callExpression.Parameters.Select(Bind).ToList();
            var func = functions.GetValueOrNone(callExpression.Name).Map(f => (BoundCallExpression)new BoundBuiltInFunctionCallExpression(f, parameters));
            var proc = procedures.GetValueOrNone(callExpression.Name).Map(procedure => (BoundCallExpression)new BoundProcedureCallExpression(procedure, parameters));

            var funcOrPro = func.Else(() => proc);

            funcOrPro.MatchNone(() =>
                errors.Add(new BinderError(ErrorKind.UndeclaredFunctionOrProcedure, callExpression.Position, callExpression.Name)));

            return funcOrPro.ValueOr(new BoundEmptyCallExpression());
        }

        private BoundStatement Bind(IfStatement ifStatement)
        {
            return new BoundIfStatement(Bind(ifStatement.Condition), Bind(ifStatement.TrueBlock), ifStatement.FalseBlock.Map(Bind));
        }

        private BoundBlock Bind(Block block)
        {
            return new(block.Statements.Select(Bind).ToList());
        }

        private BoundStatement Bind(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message);
        }

        private BoundStatement Bind(CallStatement callStatement)
        {
            return new BoundCallStatement(Bind(callStatement.Call));
        }
    }
}