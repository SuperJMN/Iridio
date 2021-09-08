using System.Collections.Generic;
using System.Linq;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundProcedureCallExpression : BoundCallExpression
    {
        public ProcedureSymbol ProcedureSymbol { get; }
        public IReadOnlyCollection<BoundExpression> Parameters { get; }

        public BoundProcedureCallExpression(ProcedureSymbol procedureSymbol, IEnumerable<BoundExpression> parameters, Position position) :
            base(position)
        {
            ProcedureSymbol = procedureSymbol;
            Parameters = parameters.ToList();
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}