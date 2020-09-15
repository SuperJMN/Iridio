using System;
using System.Collections.Generic;
using MoreLinq.Extensions;
using Optional.Collections;

namespace SimpleScript.Zafiro
{
    public static class EnumerableExtensions
    {
        public static void WhenMiddleAndLast<T>(this ICollection<T> collection, Action<T> nonLast, Action<T> last)
        {
            collection.SkipLast(1)
                .ForEach(nonLast);

            collection.LastOrNone().MatchSome(last);
        }
    }
}