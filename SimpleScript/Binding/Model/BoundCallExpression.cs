using System.Collections.Generic;

namespace SimpleScript.Binding.Model
{
    public class BoundCallExpression : BoundExpression
    {
        public BoundFunctionDeclaration FunctionDeclaration { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundCallExpression(BoundFunctionDeclaration functionDeclaration, IEnumerable<BoundExpression> parameters)
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