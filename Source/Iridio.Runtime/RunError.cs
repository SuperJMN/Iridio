using System.Collections.Generic;

namespace Iridio.Runtime
{
    public abstract class RunError
    {
        public abstract IEnumerable<string> Items { get; }
    }
}