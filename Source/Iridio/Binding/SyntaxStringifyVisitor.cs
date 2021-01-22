using System.Globalization;
using Iridio.Common.Utils;
using Iridio.Parsing.Model;
using MoreLinq;

namespace Iridio.Binding
{
    public class SyntaxStringifyVisitor : IExpressionVisitor
    {
        private readonly IStringAssistant sa = new LineEatingStringAssistant(new StringAssistant());

        public void Visit(IridioSyntax boundScript)
        {
            boundScript.Procedures.ForEach(function =>
            {
                function.Accept(this);
            });
        }

        public void Visit(AssignmentStatement a)
        {
            sa.TabPrint(a.Variable + " = ");
            a.Expression.Accept(this);
            sa.Print(";");
        }

        public void Visit(Condition c)
        {
            sa.Print("(");
            c.Left.Accept(this);
            sa.Print(" " + c.Op.Op + " ");
            c.Right.Accept(this);
            sa.Print(")");
        }

        public void Visit(DoubleExpression doubleExpression)
        {
            sa.Print(doubleExpression.Value.ToString(CultureInfo.InvariantCulture));
        }

        public void Visit(EchoStatement echo)
        {
            sa.TabPrint($"<{echo.Message}>");
        }

        public void Visit(IfStatement ifs)
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

        public void Visit(Procedure fd)
        {
            sa.Print(fd.Name);
            fd.Block.Accept(this);
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

        public void Visit(IntegerExpression ne)
        {
            sa.Print(ne.Value.ToString());
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

        public void Visit(CallStatement st)
        {
            sa.TabPrint("");
            st.Call.Accept(this);
            sa.Print(";");
        }

        public void Visit(StringExpression strExpr)
        {
            sa.Print("\"" + strExpr.String + "\"");

        }

        public void Visit(IdentifierExpression identifier)
        {
            sa.Print(identifier.Identifier);

        }

        public override string ToString()
        {
            return sa.ToString();
        }
    }
}