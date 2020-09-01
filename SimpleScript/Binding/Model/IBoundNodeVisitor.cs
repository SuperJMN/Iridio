using SimpleScript.Parsing.Model;

namespace SimpleScript.Binding.Model
{
    public interface IBoundNodeVisitor
    {
        void Visit(BoundScript boundScript);
        void Visit(BoundAssignmentStatement assignment);
        void Visit(BoundCondition boundScript);
        void Visit(BoundEchoStatement echo);
        void Visit(BoundIfStatement boundScript);
        void Visit(BoundFunction function);
        void Visit(BoundBlock block);
        void Visit(BoundNumericExpression numericExpression);
        void Visit(BoundCallExpression callExpression);
        void Visit(BoundCallStatement call);
        void Visit(BoundIdentifier boundIdentifier);
        void Visit(BoundStringExpression identifier);
    }
}