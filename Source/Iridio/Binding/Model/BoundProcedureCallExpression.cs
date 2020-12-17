using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class BoundProcedureCallExpression : BoundCallExpression
    {
        public BoundProcedure Procedure { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundProcedureCallExpression(BoundProcedure procedure, IEnumerable<BoundExpression> parameters)
        {
            Procedure = procedure;
            Parameters = parameters;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}