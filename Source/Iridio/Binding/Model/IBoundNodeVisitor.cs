namespace Iridio.Binding.Model
{
    public interface IBoundNodeVisitor
    {
        void Visit(Script script);
        void Visit(BoundAssignmentStatement assignmentStatement);
        void Visit(BoundEchoStatement echo);
        void Visit(BoundIfStatement ifStatement);
        void Visit(BoundProcedure procedure);
        void Visit(BoundBlock block);
        void Visit(BoundIntegerExpression integerExpression);
        void Visit(BoundCallStatement callStatement);
        void Visit(BoundIdentifier identifier);
        void Visit(BoundStringExpression stringExpression);
        void Visit(BoundFunctionCallExpression functionCallExpression);
        void Visit(BoundProcedureCallExpression procedureCallExpression);
        void Visit(BoundDoubleExpression doubleExpression);
        void Visit(BoundBinaryExpression binaryExpression);
        void Visit(BoundBooleanValueExpression booleanValueExpression);
        void Visit(BoundUnaryExpression unaryExpression);
        void Visit(BoundProcedureSymbolCallExpression procedureCallExpression);
    }
}