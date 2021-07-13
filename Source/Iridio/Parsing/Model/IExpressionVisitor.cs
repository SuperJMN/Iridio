namespace Iridio.Parsing.Model
{
    public interface IExpressionVisitor
    {
        void Visit(IntegerExpression integerExpression);
        void Visit(IdentifierExpression identifierExpression);
        void Visit(StringExpression stringExpression);
        void Visit(CallExpression callExpression);
        void Visit(AssignmentStatement assignmentStatement);
        void Visit(IfStatement ifStatement);
        void Visit(CallStatement callStatement);
        void Visit(IridioSyntax iridioSyntax);
        void Visit(EchoStatement echoStatement);
        void Visit(Block block);
        void Visit(Procedure procedure);
        void Visit(DoubleExpression doubleExpression);
        void Visit(BinaryExpression binaryExpression);
        void Visit(UnaryExpression unaryExpression);
        void Visit(BooleanValueExpression booleanValueExpression);
    }
}