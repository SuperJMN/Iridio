using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using SimpleScript.Zafiro;

namespace SimpleScript.Binding
{
    public class BoundNodeStringifyVisitor : IBoundNodeVisitor
    {
        readonly StringAssistant stringAssistant = new StringAssistant();

        public void Visit(BoundScript boundScript)
        {
        }

        public void Visit(BoundAssignmentStatement assignment)
        {
        }

        public void Visit(BoundCondition boundScript)
        {
        }

        public void Visit(BoundEchoStatement echo)
        {
        }

        public void Visit(BoundIfStatement boundScript)
        {
        }

        public void Visit(BoundFunctionDeclaration functionDeclaration)
        {
        }

        public void Visit(BoundBlock block)
        {
        }

        public void Visit(BoundNumericExpression numericExpression)
        {
        }

        public void Visit(BoundBuiltInFunctionCallExpression functionDeclaration)
        {
        }

        public void Visit(BoundCustomCallExpression callExpression)
        {
        }

        public void Visit(BoundCallStatement st)
        {
        }

        public void Visit(BoundIdentifier boundIdentifier)
        {
        }

        public void Visit(BoundStringExpression identifier)
        {
        }

        public override string ToString()
        {
            return stringAssistant.ToString();
        }
    }

    public class SyntaxStringifyVisitor : IExpressionVisitor
    {
        private readonly IStringAssistant sa = new LineEatingStringAssistant(new StringAssistant());

        public void Visit(EnhancedScript boundScript)
        {
            boundScript.Functions.ForEach(function => function.Accept(this));
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
            block.Statements.ForEach(st => st.Accept(this));
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
        }

        public void Visit(CallStatement st)
        {
        }

        public void Visit(StringExpression strExpr)
        {
            sa.Print(strExpr.String);

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