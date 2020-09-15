using SimpleScript.Binding.Model;
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
}