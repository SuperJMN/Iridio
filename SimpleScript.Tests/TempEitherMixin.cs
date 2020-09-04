using System.Threading.Tasks;
using Optional.Unsafe;
using Zafiro.Core.Patterns;

namespace SimpleScript.Tests
{
    public static class TempEitherMixin
    {
        public static async Task<Either<TLeft, TRight>> AwaitRight<TLeft, TRight>(this Either<TLeft, Task<TRight>> either)
        {
            if (either.IsRight)
            {
                return await either.RightValue.ValueOrFailure();
            }
            else
            {
                return either.LeftValue.ValueOrFailure();
            }
        }
    }
}