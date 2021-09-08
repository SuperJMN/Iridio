using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundFunctionCallExpression : BoundCallExpression
    {
        public INamed Function { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundFunctionCallExpression(INamed function, IEnumerable<BoundExpression> parameters, Position position) : base(position)
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