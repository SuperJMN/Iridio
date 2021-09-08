using System;
using CSharpFunctionalExtensions;

namespace Iridio.Binding
{
    public static class FunctionalExtensions
    {
        public static void ExecuteOnEmpty<T>(this Maybe<T> maybe, Action action)
        {
            if (maybe.HasNoValue)
            {
                action();
            }
        }
    }
}