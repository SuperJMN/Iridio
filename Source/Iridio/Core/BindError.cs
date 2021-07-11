using System.Linq;
using Iridio.Binding;

namespace Iridio.Core
{
    public class BindError : CompilerError
    {
        public BinderErrors Errors { get; }

        public BindError(BinderErrors errors)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return string.Join(";", Errors.Select(x => x.ToString()));
        }
    }
}