using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Binding
{
    public interface IBinder
    {
        Either<Errors, BoundScript> Bind(EnhancedScript script);
    }
}