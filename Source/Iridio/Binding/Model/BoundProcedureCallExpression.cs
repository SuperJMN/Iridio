using System.Collections.Generic;
using System.Linq;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundProcedureCallExpression : BoundCallExpression
    {
        public BoundProcedure Procedure { get; }
        public IReadOnlyCollection<BoundExpression> Parameters { get; }

        public BoundProcedureCallExpression(BoundProcedure procedure, IEnumerable<BoundExpression> parameters, Position position) : base(position)
        {
            Procedure = procedure;
            Parameters = parameters.ToList();
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}