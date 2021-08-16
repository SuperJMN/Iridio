using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundBuiltInFunctionCallExpression : BoundCallExpression
    {
        public INamed Function { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundBuiltInFunctionCallExpression(INamed function, IEnumerable<BoundExpression> parameters, Position position) : base(position)
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