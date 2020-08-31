using System;
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
        }

        public void Visit(BoundEchoStatement echo)
        {
            stringAssistant.AppendLine($"<{echo.Message}>");
        }

        public void Visit(BoundIfStatement boundScript)
        {
            stringAssistant.AppendLine("(if statement)");
        }

        public void Visit(BoundFunction function)
        {
            stringAssistant.IndentedAppendLine(function.Name);
            function.Block.Accept(this);
        }

        public void Visit(BoundBlock block)
        {
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

        public override string ToString()
        {
            return stringAssistant.ToString();
        }
    }
}