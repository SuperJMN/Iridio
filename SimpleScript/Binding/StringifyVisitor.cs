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
            boundScript.Functions.ForEach(function => function.Accept(this));
        }

        public void Visit(BoundAssignmentStatement assignment)
        {
            stringAssistant.Append((AppendableString)$"{assignment.Variable} = ");
            assignment.Expression.Accept(this);
            stringAssistant.Append(";");
        }

        public void Visit(BoundCondition boundScript)
        {
            stringAssistant.Append("(");
            boundScript.Left.Accept(this);
            stringAssistant.Append(" " + boundScript.Op.Op + " ");
            boundScript.Right.Accept(this);
            stringAssistant.Append(")");
        }

        public void Visit(BoundEchoStatement echo)
        {
            stringAssistant.AppendLine($"<{echo.Message}>");
        }

        public void Visit(BoundIfStatement boundScript)
        {
            stringAssistant.Append((AppendableString)"if ");
            boundScript.Condition.Accept(this);

            boundScript.TrueBlock.Accept(this);
            boundScript.FalseBlock.MatchSome(block =>
            {
                stringAssistant.NewLineWith("else");
                block.Accept(this);
            });
        }

        public void Visit(BoundFunctionDeclaration functionDeclaration)
        {
            stringAssistant.Append(functionDeclaration.Name);
            functionDeclaration.Block.Accept(this);
        }

        public void Visit(BoundBlock block)
        {
            stringAssistant.NewLineWith("{");
            stringAssistant.IncreaseIndent();
            block.BoundStatements.ForEach(statement =>
            {
                statement.Accept(this);
            });
            stringAssistant.DecreaseIndent();
            stringAssistant.AppendLine("");
            stringAssistant.NewLineWith("}");
        }

        public void Visit(BoundNumericExpression numericExpression)
        {
            stringAssistant.Append(numericExpression.Value.ToString());
        }

        public void Visit(BoundBuiltInFunctionCallExpression functionDeclaration)
        {
            stringAssistant.Append($"Call to {functionDeclaration.Function.Name}");
        }

        public void Visit(BoundCustomCallExpression callExpression)
        {
            stringAssistant.Append($"Call to {callExpression.FunctionDeclaration.Name}");
        }

        public void Visit(BoundCallStatement st)
        {
            stringAssistant.AppendLine("Call statement: ");
            st.Call.Accept(this);
        }

        public void Visit(BoundIdentifier boundIdentifier)
        {
            stringAssistant.Append(boundIdentifier.Identifier);
        }

        public void Visit(BoundStringExpression identifier)
        {
            stringAssistant.Append(identifier.String);
        }

        public override string ToString()
        {
            return stringAssistant.ToString();
        }
    }

    public class SyntaxStringifyVisitor : IExpressionVisitor
    {
        private readonly StringAssistant stringAssistant = new StringAssistant();

        public void Visit(EnhancedScript boundScript)
        {
            boundScript.Functions.ForEach(function => function.Accept(this));
        }

        public void Visit(AssignmentStatement assignment)
        {
            stringAssistant.Begin();
            stringAssistant.
            assignment.Expression.Accept(this);
            stringAssistant.End(";");
        }

        public void Visit(Condition boundScript)
        {
            stringAssistant.Add("(");
            boundScript.Left.Accept(this);
            stringAssistant.Add(" " + boundScript.Op.Op + " ");
            boundScript.Right.Accept(this);
            stringAssistant.Add(")");
        }

        public void Visit(EchoStatement echo)
        {
            stringAssistant.AppendLine($"<{echo.Message}>");
        }

        public void Visit(IfStatement boundScript)
        {
           
        }

        public void Visit(FunctionDeclaration functionDeclaration)
        {
            stringAssistant.Append(functionDeclaration.Name);
            functionDeclaration.Block.Accept(this);
        }

        public void Visit(Block block)
        {
            stringAssistant.NewLineWith("{");
            stringAssistant.IncreaseIndent();
            block.Statements.ForEach(statement =>
            {
                statement.Accept(this);
            });
            stringAssistant.DecreaseIndent();
            stringAssistant.NewLineWith("}");
        }

        public void Visit(NumericExpression numericExpression)
        {
            stringAssistant.Append(numericExpression.Value.ToString());
        }

        public void Visit(CallExpression callExpression)
        {
            stringAssistant.Append($"Call to {callExpression.Name}");
        }

        public void Visit(CallStatement st)
        {
            stringAssistant.AppendLine("Call statement: ");
            st.Call.Accept(this);
        }

        public void Visit(StringExpression identifier)
        {
            stringAssistant.Append(identifier.String);
        }

        public void Visit(IdentifierExpression boundIdentifier)
        {
            stringAssistant.Append(boundIdentifier.Identifier);
        }

        public override string ToString()
        {
            return stringAssistant.ToString();
        }
    }
}