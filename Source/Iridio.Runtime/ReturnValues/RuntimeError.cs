using System.Collections.Generic;

namespace Iridio.Runtime.ReturnValues
{
    public abstract class RuntimeError
    {
        public abstract IEnumerable<string> Items { get; }
    }
}