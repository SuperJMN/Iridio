using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Iridio.Runtime.ReturnValues
{
    public class RuntimeErrors : Collection<RuntimeError>
    {
        public RuntimeErrors(params RuntimeError[] concat) : base(concat.ToList())
        {
        }

        public RuntimeErrors(IEnumerable<RuntimeError> items) : base(items.ToList())
        {
        }

        public static RuntimeErrors Concat(RuntimeErrors a, RuntimeErrors b) => new RuntimeErrors(a.Concat(b));
    }
}