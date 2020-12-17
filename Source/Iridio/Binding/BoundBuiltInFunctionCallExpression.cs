using System.Collections.Generic;
using Iridio.Binding.Model;
using Iridio.Common;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Binding
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