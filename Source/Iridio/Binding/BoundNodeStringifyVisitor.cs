using System.Globalization;
using System.Linq;
using Iridio.Binding.Model;
using Iridio.Common.Utils;
using MoreLinq;

namespace Iridio.Binding
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

        public void Visit(BoundAssignmentStatement a)
        {
            sa.TabPrint(a.Variable + " = ");
            a.Expression.Accept(this);
            sa.Print(";");
        }

        public void Visit(BoundCondition c)
        {
            sa.Print("(");
            c.Left.Accept(this);
            sa.Print(" " + c.Op.Op + " ");
            c.Right.Accept(this);
            sa.Print(")");
        }

        public void Visit(BoundEchoStatement echo)
        {
        }

        public void Visit(BoundIfStatement ifs)
        {
            sa.TabPrint("if ");
            ifs.Condition.Accept(this);
            ifs.TrueBlock.Accept(this);
            ifs.FalseBlock.MatchSome(b =>
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

        public void Visit(BoundCallStatement st)
        {
            sa.TabPrint("");
            st.Call.Accept(this);
            sa.Print(";");
        }

        public void Visit(BoundBuiltInFunctionCallExpression functionDeclaration)
        {
            sa.Print(functionDeclaration.Function.Name + "(");
            functionDeclaration.Parameters.ToList().ForEach(ex =>
            {
                ex.Accept(this);
                sa.Print(", ");
            }, ex => ex.Accept(this));
            sa.Print(")");
        }

        public void Visit(BoundProcedureCallExpression callExpression)
        {
            sa.Print(callExpression.Procedure.Name + "(");
            callExpression.Parameters.ToList().ForEach(ex =>
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

        public void Visit(BoundBinaryExpression functionDeclaration)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BoundIdentifier boundIdentifier)
        {
            sa.Print(boundIdentifier.Identifier);
        }

        public void Visit(BoundStringExpression strExpr)
        {
            sa.Print("\"" + strExpr.String + "\"");
        }

        public override string ToString()
        {
            return sa.ToString();
        }
    }
}