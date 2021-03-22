using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Iridio.Runtime
{
    public class RuntimeErrors : Collection<RunError>
    {
        public RuntimeErrors(params RunError[] concat) : base(concat.ToList())
        {
        }

        public RuntimeErrors(IEnumerable<RunError> items) : base(items.ToList())
        {
        }

        public static RuntimeErrors Concat(RuntimeErrors a, RuntimeErrors b) => new RuntimeErrors(a.Concat(b));
    }
}