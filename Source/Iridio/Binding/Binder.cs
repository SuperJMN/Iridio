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
        private readonly Collection<Error> errors = new Collection<Error>();

        private readonly IDictionary<string, BoundProcedure> procedures =
            new Dictionary<string, BoundProcedure>();

        private readonly ISet<string> initializedVariables = new HashSet<string>();

        public Binder(IEnumerable<IFunctionDeclaration> functionDeclarations)
        {
            if (functionDeclarations == null) throw new ArgumentNullException(nameof(functionDeclarations));
            functions = functionDeclarations.ToDictionary(d => d.Name, d => d);
        }

        public Either<Errors, Script> Bind(IridioSyntax syntax)
        {
            procedures.Clear();
            initializedVariables.Clear();
            errors.Clear();

            var procs = syntax.Procedures.Select(Bind);
            var boundProcedures = procs.ToList();
            var main = boundProcedures.FirstOrNone(b => b.Name == "Main");
            main.MatchNone(() => errors.Add(new Error(ErrorKind.UndefinedMainFunction, "Main procedure is undefined")));

            if (errors.Any())
            {
                return Either.Error<Errors, Script>(new Errors(errors));
            }

            var script = new Script(boundProcedures, main.ValueOrFailure());
            return Either.Success<Errors, Script>(script);
        }

        private BoundProcedure Bind(Procedure proc)
        {
            if (functions.ContainsKey(proc.Name))
            {
                AddError(new Error(ErrorKind.ProcedureNameConflictsWithBuiltInFunction, proc.Name));
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
            switch (expression)
            {
                case CallExpression callExpression:
                    return Bind(callExpression);
                case IdentifierExpression identifierExpression:
                    return Bind(identifierExpression);

                case NumericExpression numericExpression:
                    return Bind(numericExpression);
                case StringExpression stringExpression:
                    return Bind(stringExpression);
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }

        private BoundExpression Bind(StringExpression stringExpression)
        {
            var str = stringExpression.String;
            //LookupUninitializedReferences(str);

            return new BoundStringExpression(str);
        }

        private void LookupUninitializedReferences(string str)
        {
            var references = References.FromString(str);
            var notInitialized = references.Where(s => !initializedVariables.Contains(s));
            notInitialized.ForEach(s => AddError(new Error(ErrorKind.ReferenceToUninitializedVariable, s)));
        }

        private BoundExpression Bind(NumericExpression numericExpression)
        {
            return new BoundNumericExpression(numericExpression.Value);
        }

        private BoundExpression Bind(IdentifierExpression identifierExpression)
        {
            //if (!initializedVariables.Contains(identifierExpression.Identifier))
            //{
            //    AddError(new Error(ErrorKind.ReferenceToUninitializedVariable, identifierExpression.Identifier));
            //}

            return new BoundIdentifier(identifierExpression.Identifier);
        }

        private void AddError(Error error)
        {
            errors.Add(error);
        }

        private BoundCallExpression Bind(CallExpression callExpression)
        {
            var parameters = callExpression.Parameters.Select(Bind).ToList();
            var func = functions.GetValueOrNone(callExpression.Name).Map(f => (BoundCallExpression)new BoundBuiltInFunctionCallExpression(f, parameters));
            var proc = procedures.GetValueOrNone(callExpression.Name).Map(procedure => (BoundCallExpression)new BoundProcedureCallExpression(procedure, parameters));

            var funcOrPro = func.Else(() => proc);

            funcOrPro.MatchNone(() => errors.Add(new Error(ErrorKind.UndeclaredFunctionOrProcedure, callExpression.Name)));

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

        private BoundCondition Bind(Condition ifStatementCondition)
        {
            return new BoundCondition(Bind(ifStatementCondition.Left), ifStatementCondition.Op, Bind(ifStatementCondition.Right));
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

    internal class BoundEmptyCallExpression : BoundCallExpression
    {
        public override void Accept(IBoundNodeVisitor visitor)
        {
        }
    }
}