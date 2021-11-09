using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MoreLinq.Extensions;

namespace Iridio.Common.Utils
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this ICollection<T> collection, Action<T> nonLast, Action<T> last)
        {
            collection.SkipLast(1)
                .ForEach(nonLast);

            collection.TryLast().Execute(last);
        }
    }
}