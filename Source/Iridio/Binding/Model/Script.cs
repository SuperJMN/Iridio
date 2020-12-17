using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class Script : IBoundNode
    {
        public IEnumerable<BoundProcedure> Procedures { get; }
        public BoundProcedure MainProcedure { get; }

        public Script(IEnumerable<BoundProcedure> procedures, BoundProcedure mainProcedure)
        {
            Procedures = procedures;
            MainProcedure = mainProcedure;
        }

        public override string ToString()
        {
            return string.Join("\n", Procedures);
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}