using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class BoundBuiltInFunctionCallExpression : BoundCallExpression
    {
        public IFunctionDeclaration Function { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundBuiltInFunctionCallExpression(IFunctionDeclaration function, IEnumerable<BoundExpression> parameters)
        {
            Function = function;
            Parameters = parameters;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}