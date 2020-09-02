using SimpleScript.Binding.Model;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns;

namespace SimpleScript.Binding
{
    public interface IBinder
    {
        Either<ErrorList, BoundScript> Bind(EnhancedScript script);
    }
}