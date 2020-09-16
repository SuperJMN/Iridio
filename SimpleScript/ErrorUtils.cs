using System;
using System.Linq;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript
{
    public static class ErrorUtils
    {
        public static Errors Concat(this Errors list, Errors errors)
        {
            return new Errors(Enumerable.Concat(list, errors));
        }

        public static string Join(Errors list)
        {
            return string.Join(Environment.NewLine, list);
        }
    }
}