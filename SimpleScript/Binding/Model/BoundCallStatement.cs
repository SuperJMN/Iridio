using System.Collections.Generic;

namespace SimpleScript.Binding.Model
{
    public class BoundCallStatement : BoundStatement
    {
        public BoundFunction Function { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundCallStatement(BoundFunction function, IEnumerable<BoundExpression> parameters)
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