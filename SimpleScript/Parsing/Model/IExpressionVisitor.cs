namespace SimpleScript.Parsing.Model
{
    public interface IExpressionVisitor
    {
        void Visit(NumericExpression identifierExpression);
        void Visit(IdentifierExpression identifierExpression);
        void Visit(StringExpression identifierExpression);
        void Visit(CallExpression identifierExpression);
        void Visit(AssignmentStatement identifierExpression);
        void Visit(IfStatement identifierExpression);
        void Visit(CallStatement identifierExpression);
        void Visit(EnhancedScript identifierExpression);
        void Visit(EchoStatement echoStatement);
        void Visit(Block block);
        void Visit(FunctionDeclaration identifierExpression);
        void Visit(Condition identifierExpression);
    }
}