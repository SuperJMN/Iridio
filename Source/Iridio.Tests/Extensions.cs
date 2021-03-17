using System;
using System.Collections.Generic;
using DynamicData.Annotations;
using DynamicData.Binding;
using Optional;
using Optional.Unsafe;

namespace Iridio.Tests
{
    public static class Extensions
    {
        public static IEnumerable<T> Correlate<T>(this IEnumerable<T> source, Func<T, T, bool> canGoTogether)
        {
            using (var enumerator = source.GetEnumerator())
            {
                var prev = Option.None<T>();
                while (enumerator.MoveNext())
                {
                    if (!prev.HasValue)
                    {
                        yield return enumerator.Current;
                    }
                    else if (prev.HasValue && canGoTogether(prev.ValueOrFailure(), enumerator.Current))
                    {
                        yield return enumerator.Current;
                    }

                    prev = enumerator.Current.Some();
                }
            }
        }
    }
}