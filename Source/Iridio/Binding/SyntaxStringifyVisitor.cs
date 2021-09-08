using System;
using System.Globalization;
using CSharpFunctionalExtensions;
using Iridio.Common.Utils;
using Iridio.Parsing.Model;
using MoreLinq;

namespace Iridio.Binding
{
    public class SyntaxStringifyVisitor : IExpressionVisitor
    {
        private readonly IStringAssistant sa = new LineEatingStringAssistant(new StringAssistant());

        public void Visit(IridioSyntax iridioSyntax)
        {
            iridioSyntax.Procedures.ForEach(function => { function.Accept(this); });
        }

        public void Visit(AssignmentStatement assignmentStatement)
        {
            sa.TabPrint(assignmentStatement.Target + " = ");
            assignmentStatement.Expression.Accept(this);
            sa.Print(";");
        }

        public void Visit(BinaryExpression binaryExpression)
        {
            binaryExpression.Left.Accept(this);
            sa.Print(" " + binaryExpression.Op.Symbol + " ");
            binaryExpression.Right.Accept(this);
        }

        public void Visit(UnaryExpression unaryExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(ConstantExpression constantExpression)
        {
            var v = constantExpression.Value;

            switch (v)
            {
                case int i:
                    sa.Print(i.ToString());
                    break;
                case double d:
                    sa.Print(d.ToString(CultureInfo.InvariantCulture));
                    break;
                case bool b:
                    sa.Print(b.ToString(CultureInfo.InvariantCulture));
                    break;
                case string str:
                    sa.Print($"\"{str}\"");
                    break;
                default:
                    throw new InvalidOperationException($"Invalid constant type {v.GetType()}");
            }
        }

        public void Visit(EchoStatement echo)
        {
            sa.TabPrint($"'{echo.Message}'");
        }

        public void Visit(IfStatement ifStatement)
        {
            sa.TabPrint("if (");
            ifStatement.Condition.Accept(this);
            sa.Print(")");
            ifStatement.TrueBlock.Accept(this);
            ifStatement.FalseBlock.Execute(b =>
            {
                sa.NewLine();
                sa.TabPrint("else");
                sa.NewLine();
                b.Accept(this);
            });
        }

        public void Visit(Procedure procedure)
        {
            sa.Print(procedure.Name);
            procedure.Block.Accept(this);
        }

        public void Visit(Block block)
        {
            sa.NewLine();
            sa.TabPrint("{");
            sa.NewLine();

            sa.IncreaseIndent();
            block.Statements.ForEach(st =>
            {
                st.Accept(this);
                sa.NewLine();
            }, st => st.Accept(this));

            sa.DecreaseIndent();

            sa.NewLine();
            sa.TabPrint("}");
            sa.NewLine();
        }

        public void Visit(CallExpression callExpression)
        {
            sa.Print(callExpression.Name + "(");
            callExpression.Parameters.ForEach(ex =>
            {
                ex.Accept(this);
                sa.Print(", ");
            }, ex => ex.Accept(this));
            sa.Print(")");
        }

        public void Visit(CallStatement callStatement)
        {
            sa.TabPrint("");
            callStatement.Call.Accept(this);
            sa.Print(";");
        }

        public void Visit(IdentifierExpression identifierExpression)
        {
            sa.Print(identifierExpression.Identifier);
        }

        public override string ToString()
        {
            return sa.ToString();
        }
    }
}