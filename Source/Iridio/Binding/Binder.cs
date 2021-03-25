using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing.Model;
using MoreLinq;
using Optional.Collections;
using Optional.Unsafe;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Binding
{
    public class Binder : IBinder
    {
        private readonly IDictionary<string, IFunctionDeclaration> functions;
        private readonly Collection<BinderError> errors = new Collection<BinderError>();

        private readonly IDictionary<string, BoundProcedure> procedures =
            new Dictionary<string, BoundProcedure>();

        private readonly ISet<string> initializedVariables = new HashSet<string>();

        public Binder(IEnumerable<IFunctionDeclaration> functionDeclarations)
        {
            if (functionDeclarations == null) throw new ArgumentNullException(nameof(functionDeclarations));
            functions = functionDeclarations.ToDictionary(d => d.Name, d => d);
        }

        public Either<BinderErrors, Script> Bind(IridioSyntax syntax)
        {
            procedures.Clear();
            initializedVariables.Clear();
            errors.Clear();

            var procs = syntax.Procedures.Select(Bind);
            var boundProcedures = procs.ToList();
            var main = boundProcedures.FirstOrNone(b => b.Name == "Main");
            main.MatchNone(() => errors.Add(new BinderError(ErrorKind.UndefinedMainFunction, "Main procedure is undefined")));

            if (errors.Any())
            {
                return Either.Error<BinderErrors, Script>(new BinderErrors(errors));
            }

            var script = new Script(boundProcedures, main.ValueOrFailure());
            return Either.Success<BinderErrors, Script>(script);
        }

        private BoundProcedure Bind(Procedure proc)
        {
            if (functions.ContainsKey(proc.Name))
            {
                AddError(new BinderError(ErrorKind.ProcedureNameConflictsWithBuiltInFunction, proc.Name));
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
            initializedVariables.Add(assignmentStatement.Variable);

            return new BoundAssignmentStatement(assignmentStatement.Variable, Bind(assignmentStatement.Expression));
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
                UnaryExpression unaryExpression => throw new NotImplementedException(),
                DoubleExpression doubleExpression => Bind(doubleExpression),
                _ => throw new ArgumentOutOfRangeException(nameof(expression))
            };
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

        private void LookupUninitializedReferences(string str)
        {
            var references = References.FromString(str);
            var notInitialized = references.Where(s => !initializedVariables.Contains(s));
            notInitialized.ForEach(s => AddError(new BinderError(ErrorKind.ReferenceToUninitializedVariable, s)));
        }

        private BoundExpression Bind(IntegerExpression integerExpression)
        {
            return new BoundIntegerExpression(integerExpression.Value);
        }

        private BoundExpression Bind(IdentifierExpression identifierExpression)
        {
            //if (!initializedVariables.Contains(identifierExpression.Identifier))
            //{
            //    AddError(new Error(ErrorKind.ReferenceToUninitializedVariable, identifierExpression.Identifier));
            //}

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

            funcOrPro.MatchNone(() => errors.Add(new BinderError(ErrorKind.UndeclaredFunctionOrProcedure, callExpression.Name)));

            return funcOrPro.ValueOr(new BoundEmptyCallExpression());
        }

        private BoundStatement Bind(IfStatement ifStatement)
        {
            return new BoundIfStatement(Bind(ifStatement.Condition), Bind(ifStatement.TrueBlock), ifStatement.FalseBlock.Map(Bind));
        }

        private BoundBlock Bind(Block block)
        {
            return new BoundBlock(block.Statements.Select(Bind));
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