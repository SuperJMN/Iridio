using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Core;
using Iridio.Parsing.Model;
using Zafiro.Core.Patterns;

namespace Iridio.Binding
{
    public class ProcedureSymbol
    {
        public ProcedureSymbol(string name, Position position)
        {
            Name = name;
            Position = position;
        }


        public string Name { get; }
        public Position Position { get; }
    }

    public class NewBinder : IBinder
    {
        private readonly ICollection<BinderError> errors = new List<BinderError>();
        private readonly Dictionary<string, ProcedureSymbol> procedures = new();
        private readonly IDictionary<string, INamed> functions;

        public NewBinder(IEnumerable<INamed> externalFunctions)
        {
            if (externalFunctions == null)
            {
                throw new ArgumentNullException(nameof(externalFunctions));
            }

            functions = externalFunctions.ToDictionary(d => d.Name, d => d);
        }

        public Result<Script, BinderErrors> Bind(IridioSyntax syntax)
        {
            DeclareProcedures(syntax);

            var procedures = BindProcedures(syntax);
            return new Script(procedures);
        }

        private void DeclareProcedures(IridioSyntax root)
        {
            foreach (var syntax in root.Procedures)
            {
                if (procedures.TryGetValue(syntax.Name, out var existingProcedure))
                {
                    errors.Add(new BinderError(ErrorKind.ProcedureAlreadyDeclared, syntax.Position,
                        $"Duplicate definition of procedure '{existingProcedure}'"));
                }
                else
                {
                    var symbol = new ProcedureSymbol(syntax.Name, syntax.Position);
                    procedures.Add(symbol.Name, symbol);
                }
            }
        }

        private IEnumerable<BoundProcedure> BindProcedures(IridioSyntax syntax)
        {
            return syntax.Procedures.Select(BindProcedure);
        }

        private BoundProcedure BindProcedure(Procedure procedure)
        {
            return new BoundProcedure(procedure.Name, BindBlock(procedure.Block), procedure.Position);
        }

        private BoundBlock BindBlock(Block block)
        {
            return new BoundBlock(block.Statements.Select(BindStatement), block.Position);
        }

        private BoundStatement BindStatement(Statement st)
        {
            return st switch
            {
                AssignmentStatement assignmentStatement => BindAssignment(assignmentStatement),
                CallStatement callStatement => BindCallStatement(callStatement),
                EchoStatement echoStatement => BindEchoStatement(echoStatement),
                IfStatement ifStatement => BindIfStatement(ifStatement),
                _ => throw new InvalidOperationException()
            };
        }

        private BoundStatement BindIfStatement(IfStatement ifStatement)
        {
            var condition = BindExpression(ifStatement.Condition);
            var trueBlock = BindBlock(ifStatement.TrueBlock);
            var falseBlock = ifStatement.FalseBlock.Map(BindBlock);
            var boundIfStatement = new BoundIfStatement(condition, trueBlock, falseBlock, ifStatement.Position);
            return boundIfStatement;
        }

        private BoundStatement BindEchoStatement(EchoStatement echoStatement)
        {
            return new BoundEchoStatement(echoStatement.Message, echoStatement.Position);
        }

        private BoundStatement BindCallStatement(CallStatement callStatement)
        {
            var call = BindCallExpression(callStatement.Call);
            return new BoundCallStatement(call, call.Position);
        }

        private BoundCallExpression BindCallExpression(CallExpression callExpression)
        {
            var parameters = callExpression.Parameters.Select(BindExpression);

            var f = functions.TryGetValue(callExpression.Name)
                .Map(f => (BoundCallExpression)new BoundFunctionCallExpression(f, parameters, callExpression.Position));

            var p = procedures.TryGetValue(callExpression.Name)
                .Map(p => (BoundCallExpression)new BoundProcedureSymbolCallExpression(p, parameters, callExpression.Position));

            var called = f.Or(p);
            called.ExecuteOnEmpty(() =>
                errors.Add(new BinderError(ErrorKind.UndeclaredFunctionOrProcedure, callExpression.Position, callExpression.Name)));
            return called.Unwrap(new BoundNopCallExpression(callExpression.Position));
        }

        private BoundStatement BindAssignment(AssignmentStatement assignmentStatement)
        {
            return new BoundAssignmentStatement(assignmentStatement.Target, BindExpression(assignmentStatement.Expression),
                assignmentStatement.Position);
        }

        private BoundExpression BindExpression(Expression expression)
        {
            switch (expression)
            {
                case BinaryExpression binaryExpression:
                    break;
                case BooleanValueExpression booleanValueExpression:
                    break;
                case CallExpression callExpression:
                    break;
                case DoubleExpression doubleExpression:
                    break;
                case IdentifierExpression identifierExpression:
                    break;
                case IntegerExpression integerExpression:
                    break;
                case StringExpression stringExpression:
                    break;
                case UnaryExpression unaryExpression:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }

            throw new InvalidOperationException();
        }
    }
}