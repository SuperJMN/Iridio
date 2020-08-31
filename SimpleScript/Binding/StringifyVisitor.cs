using System;
using System.Linq;
using MoreLinq;
using SimpleScript.Binding.Model;
using SimpleScript.Zafiro;

namespace SimpleScript.Binding
{
    public class StringifyVisitor : IBoundNodeVisitor
    {
        readonly StringAssistant stringAssistant = new StringAssistant();

        public void Visit(BoundScript boundScript)
        {
            boundScript.Functions.ForEach(function => function.Accept(this));
        }

        public void Visit(BoundAssignmentStatement assignment)
        {
            stringAssistant.IndentedAppend($"{assignment.Variable} = ");
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
            stringAssistant.IndentedAppend("if ");
            boundScript.Condition.Accept(this);
            stringAssistant.AppendLine("");
            boundScript.TrueBlock.Accept(this);
            if (boundScript.FalseBlock.BoundStatements.Any())
            {
                stringAssistant.IndentedAppendLine("else");
            }
            boundScript.FalseBlock.Accept(this);
        }

        public void Visit(BoundFunction function)
        {
            stringAssistant.IndentedAppendLine(function.Name);
            function.Block.Accept(this);
        }

        public void Visit(BoundBlock block)
        {
            if (!block.BoundStatements.Any())
            {
                return;
            }

            stringAssistant.IndentedAppendLine("{");
            stringAssistant.IncreaseIndent();
            block.BoundStatements.ForEach(statement =>
            {
                statement.Accept(this);
            });
            stringAssistant.DecreaseIndent();
            stringAssistant.AppendLine("");
            stringAssistant.IndentedAppendLine("}");
        }

        public void Visit(BoundNumericExpression numericExpression)
        {
            stringAssistant.Append(numericExpression.Value.ToString());
        }

        public void Visit(BoundCallExpression callExpression)
        {
            stringAssistant.Append($"Call to {callExpression}");
        }

        public void Visit(BoundCallStatement boundScript)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoundIdentifier boundIdentifier)
        {
            stringAssistant.Append(boundIdentifier.Identifier);
        }

        public override string ToString()
        {
            return stringAssistant.ToString();
        }
    }
}