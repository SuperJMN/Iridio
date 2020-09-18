using System.Collections.Generic;
using Iridio.Binding.Model;

namespace Iridio.Binding
{
    public class BoundBuiltInFunctionCallExpression : BoundCallExpression
    {
        public IFunction Function { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundBuiltInFunctionCallExpression(IFunction function, IEnumerable<BoundExpression> parameters)
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