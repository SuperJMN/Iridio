using System.Collections.Generic;

namespace Iridio.Runtime.ReturnValues
{
    public class TypeMismatch : RuntimeError
    {
        public override IEnumerable<string> Items => new[] { this.ToString() };

        public override string ToString()
        {
            return "Type mismatch";
        }
    }
}