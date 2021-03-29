using System.Collections.ObjectModel;
using Iridio.Common;

namespace Iridio.Binding
{
    public class BinderErrors : Collection<BinderError>
    {
        public BinderErrors(Collection<BinderError> errors) : base(errors)
        {
        }
    }
}