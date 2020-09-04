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
            boundScript.FalseBlock.MatchSome(block =>
            {
                stringAssistant.IndentedAppendLine("else");
                block.Accept(this);
            });
        }

        public void Visit(BoundFunctionDeclaration functionDeclaration)
        {
            stringAssistant.IndentedAppendLine(functionDeclaration.Name);
            functionDeclaration.Block.Accept(this);
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
            stringAssistant.IndentedAppendLine("Call statement: ");
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
}