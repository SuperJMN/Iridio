namespace SimpleScript.Parsing.Model
{
    public interface IExpressionVisitor
    {
        void Visit(NumericExpression ne);
        void Visit(IdentifierExpression identifier);
        void Visit(StringExpression strExpr);
        void Visit(CallExpression identifierExpression);
        void Visit(AssignmentStatement a);
        void Visit(IfStatement ifs);
        void Visit(CallStatement identifierExpression);
        void Visit(EnhancedScript identifierExpression);
        void Visit(EchoStatement echoStatement);
        void Visit(Block block);
        void Visit(FunctionDeclaration fd);
        void Visit(Condition c);
        void Visit(Header header);
        void Visit(Declaration declaration);
    }
}