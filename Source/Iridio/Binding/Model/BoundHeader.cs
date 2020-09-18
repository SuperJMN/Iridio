using System.Collections.Generic;

namespace Iridio.Binding.Model
{
    public class BoundHeader
    {
        public BoundHeader(IEnumerable<BoundDeclaration> declarations)
        {
            Declarations = declarations;
        }

        public IEnumerable<BoundDeclaration> Declarations { get; set; }
    }
}