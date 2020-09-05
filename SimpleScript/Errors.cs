using System;
using System.Linq;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript
{
    public static class Errors
    {
        public static ErrorList Concat(this ErrorList list, ErrorList errorList)
        {
            return new ErrorList(Enumerable.Concat(list, errorList));
        }

        public static string Join(ErrorList list)
        {
            return string.Join(Environment.NewLine, list);
        }
    }
}