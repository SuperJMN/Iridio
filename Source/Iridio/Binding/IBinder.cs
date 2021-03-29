using Iridio.Binding.Model;
using Iridio.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Binding
{
    public interface IBinder
    {
        Either<BinderErrors, Script> Bind(IridioSyntax syntax);
    }
}