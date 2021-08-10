using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class BoundBuiltInFunctionCallExpression : BoundCallExpression
    {
        public INamed Function { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundBuiltInFunctionCallExpression(INamed function, IEnumerable<BoundExpression> parameters)
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