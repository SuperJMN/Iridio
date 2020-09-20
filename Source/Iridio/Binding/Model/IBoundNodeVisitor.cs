namespace Iridio.Binding.Model
{
    public interface IBoundNodeVisitor
    {
        void Visit(CompilationUnit compilationUnit);
        void Visit(BoundAssignmentStatement assignment);
        void Visit(BoundCondition boundScript);
        void Visit(BoundEchoStatement echo);
        void Visit(BoundIfStatement boundScript);
        void Visit(BoundFunctionDeclaration functionDeclaration);
        void Visit(BoundBlock block);
        void Visit(BoundNumericExpression numericExpression);
        void Visit(BoundCallStatement st);
        void Visit(BoundIdentifier boundIdentifier);
        void Visit(BoundStringExpression identifier);
        void Visit(BoundBuiltInFunctionCallExpression functionDeclaration);
        void Visit(BoundProcedureCallExpression callExpression);
    }
}