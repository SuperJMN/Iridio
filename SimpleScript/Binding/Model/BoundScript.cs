using System.Collections.Generic;

namespace SimpleScript.Binding.Model
{
    public class BoundScript
    {
        public IEnumerable<BoundFunction> Functions { get; }

        public BoundScript(IEnumerable<BoundFunction> functions)
        {
            Functions = functions;
        }
    }
}