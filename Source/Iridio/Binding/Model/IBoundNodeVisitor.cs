namespace Iridio.Binding.Model
{
    public interface IBoundNodeVisitor
    {
        void Visit(Script script);
        void Visit(BoundAssignmentStatement assignment);
        void Visit(BoundCondition boundScript);
        void Visit(BoundEchoStatement echo);
        void Visit(BoundIfStatement boundScript);
        void Visit(BoundProcedure procedure);
        void Visit(BoundBlock block);
        void Visit(BoundIntegerExpression integerExpression);
        void Visit(BoundCallStatement st);
        void Visit(BoundIdentifier boundIdentifier);
        void Visit(BoundStringExpression identifier);
        void Visit(BoundBuiltInFunctionCallExpression functionDeclaration);
        void Visit(BoundProcedureCallExpression callExpression);
        void Visit(BoundDoubleExpression doubleExpression);
        void Visit(BoundBinaryExpression functionDeclaration);
    }
}