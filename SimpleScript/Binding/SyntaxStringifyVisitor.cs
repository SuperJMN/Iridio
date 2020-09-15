using MoreLinq;
using Optional.Collections;
using SimpleScript.Parsing.Model;
using SimpleScript.Zafiro;

namespace SimpleScript.Binding
{
    public class SyntaxStringifyVisitor : IExpressionVisitor
    {
        private readonly IStringAssistant sa = new LineEatingStringAssistant(new StringAssistant());

        public void Visit(EnhancedScript boundScript)
        {
            boundScript.Functions.ForEach(function =>
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

        public void Visit(EchoStatement echo)
        {
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

        public void Visit(FunctionDeclaration fd)
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
            block.Statements.WhenMiddleAndLast(st =>
            {
                st.Accept(this);
                sa.NewLine();
            }, st => st.Accept(this));

            sa.DecreaseIndent();

            sa.NewLine();
            sa.TabPrint("}");
            sa.NewLine();
        }

        public void Visit(NumericExpression ne)
        {
            sa.Print(ne.Value.ToString());
        }

        public void Visit(CallExpression callExpression)
        {
            sa.Print(callExpression.Name + "(");
            callExpression.Parameters.WhenMiddleAndLast(ex =>
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