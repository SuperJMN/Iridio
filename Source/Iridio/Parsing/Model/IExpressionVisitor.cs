namespace Iridio.Parsing.Model
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
        void Visit(IridioSyntax identifierExpression);
        void Visit(EchoStatement echoStatement);
        void Visit(Block block);
        void Visit(Procedure fd);
        void Visit(Condition c);
    }
}