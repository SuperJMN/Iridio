using System.Collections.ObjectModel;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Binding
{
    public interface IBinder
    {
        Either<BinderErrors, Script> Bind(IridioSyntax syntax);
    }

    public class BinderErrors : Collection<BinderError>
    {
        public BinderErrors(Collection<BinderError> errors) : base(errors)
        {
        }
    }
}