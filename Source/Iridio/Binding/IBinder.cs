using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Binding
{
    public interface IBinder
    {
        Either<Errors, CompiledScript> Bind(EnhancedScript script);
    }
}