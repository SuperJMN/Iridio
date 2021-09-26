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
        void Visit(BoundCallStatement callStatement);
        void Visit(BoundReference reference);
        void Visit(BoundFunctionCallExpression functionCallExpression);
        void Visit(BoundBinaryExpression binaryExpression);
        void Visit(BoundUnaryExpression unaryExpression);
        void Visit(BoundProcedureCallExpression procedureCallExpression);
        void Visit(BoundConstantExpression boundConstantExpression);
    }
}