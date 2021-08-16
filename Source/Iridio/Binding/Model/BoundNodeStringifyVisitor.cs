using System;
using System.Globalization;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Common.Utils;
using MoreLinq;

namespace Iridio.Binding.Model
{
    public class BoundNodeStringifyVisitor : IBoundNodeVisitor
    {
        private readonly IStringAssistant sa = new LineEatingStringAssistant(new StringAssistant());

        public void Visit(Script compilationUnit)
        {
            compilationUnit.Procedures.ForEach(function =>
            {
                function.Accept(this);
            });
        }

        public void Visit(BoundAssignmentStatement assignmentStatement)
        {
            sa.TabPrint(assignmentStatement.Variable + " = ");
            assignmentStatement.Expression.Accept(this);
            sa.Print(";");
        }

        public void Visit(BoundEchoStatement echo)
        {
        }

        public void Visit(BoundIfStatement ifStatement)
        {
            sa.TabPrint("if ");
            ifStatement.Condition.Accept(this);
            ifStatement.TrueBlock.Accept(this);
            ifStatement.FalseBlock.Execute(b =>
            {
                sa.NewLine();
                sa.TabPrint("else");
                sa.NewLine();
                b.Accept(this);
            });
        }

        public void Visit(BoundProcedure fd)
        {
            sa.Print(fd.Name);
            fd.Block.Accept(this);
        }

        public void Visit(BoundBlock block)
        {
            sa.NewLine();
            sa.TabPrint("{");
            sa.NewLine();

            sa.IncreaseIndent();
            block.Statements.ToList().ForEach(st =>
            {
                st.Accept(this);
                sa.NewLine();
            }, st => st.Accept(this));

            sa.DecreaseIndent();

            sa.NewLine();
            sa.TabPrint("}");
            sa.NewLine();
        }

        public void Visit(BoundIntegerExpression ne)
        {
            sa.Print(ne.Value.ToString());
        }

        public void Visit(BoundCallStatement callStatement)
        {
            sa.TabPrint("");
            callStatement.Call.Accept(this);
            sa.Print(";");
        }

        public void Visit(BoundFunctionCallExpression functionCallExpression)
        {
            sa.Print(functionCallExpression.Function.Name + "(");
            functionCallExpression.Parameters.ToList().ForEach(ex =>
            {
                ex.Accept(this);
                sa.Print(", ");
            }, ex => ex.Accept(this));
            sa.Print(")");
        }

        public void Visit(BoundProcedureCallExpression procedureCallExpression)
        {
            sa.Print(procedureCallExpression.Procedure.Name + "(");
            procedureCallExpression.Parameters.ToList().ForEach(ex =>
            {
                ex.Accept(this);
                sa.Print(", ");
            }, ex => ex.Accept(this));
            sa.Print(")");
        }

        public void Visit(BoundDoubleExpression doubleExpression)
        {
            sa.Print(doubleExpression.Value.ToString(CultureInfo.InvariantCulture));
        }

        public void Visit(BoundBinaryExpression binaryExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoundBooleanValueExpression booleanValueExpression)
        {
            sa.Print(booleanValueExpression.Value.ToString());
        }

        public void Visit(BoundUnaryExpression unaryExpression)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoundIdentifier identifier)
        {
            sa.Print(identifier.Identifier);
        }

        public void Visit(BoundStringExpression stringExpression)
        {
            sa.Print("\"" + stringExpression.String + "\"");
        }

        public override string ToString()
        {
            return sa.ToString();
        }
    }
}