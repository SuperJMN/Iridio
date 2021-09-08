using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Iridio.Common;

namespace Iridio.Binding
{
    public class BinderErrors : Collection<BinderError>
    {
        public BinderErrors(IEnumerable<BinderError> errors) : base(errors.ToList())
        {
        }
    }
}