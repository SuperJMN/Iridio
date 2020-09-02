using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleScript.Tests
{
    public static class TaskUtils
    {
        public static async Task<IEnumerable<TResult>> AsyncSelect<TInput, TResult>(this IEnumerable<TInput> items,
            Func<TInput, Task<TResult>> selector)
        {
            var results = new List<TResult>();

            foreach (var item in items)
            {
                results.Add(await selector(item));
            }

            return results;
        }
    }
}