using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Parsing.Model;

namespace Iridio.Binding
{
    public interface IBinder
    {
        Result<Script, BinderErrors> Bind(IridioSyntax syntax);
    }
}