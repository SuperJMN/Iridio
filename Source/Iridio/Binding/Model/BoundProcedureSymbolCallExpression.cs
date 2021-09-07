using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundProcedureSymbolCallExpression : BoundCallExpression
    {
        public ProcedureSymbol Procedure { get; }
        public IEnumerable<BoundExpression> Parameters { get; }

        public BoundProcedureSymbolCallExpression(ProcedureSymbol procedure, IEnumerable<BoundExpression> parameters, Position position) :
            base(position)
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