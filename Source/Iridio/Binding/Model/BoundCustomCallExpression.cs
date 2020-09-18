using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class BoundCustomCallExpression : BoundCallExpression
    {
        public BoundFunctionDeclaration FunctionDeclaration { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundCustomCallExpression(BoundFunctionDeclaration functionDeclaration, IEnumerable<BoundExpression> parameters)
        {
            FunctionDeclaration = functionDeclaration;
            Parameters = parameters;
        }


        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}