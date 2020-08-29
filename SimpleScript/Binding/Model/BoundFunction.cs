using System.Collections.Generic;
using SimpleScript.Parsing.Model;
using Zafiro.Core.Patterns;

namespace SimpleScript.Binding
{
    public class BoundFunction
    {
        public Function Func { get; }

        public BoundFunction(Function func, Either<ErrorList, IEnumerable<BoundStatement>> either)
        {
            Func = func;
        }
    }
}