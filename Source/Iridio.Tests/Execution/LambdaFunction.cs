using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iridio.Common;
using Iridio.Parsing.Model;

namespace Iridio.Tests.Execution
{
    public class LambdaFunction<T1, T2, T3> : IFunction
    {
        public LambdaFunction(string name, Func<T1, T2, T3> func)
        {
            Name = name;
            Func = func;
        }

        public async Task<object> Invoke(object[] parameters)
        {
            var result = Func.DynamicInvoke(parameters);
            return (T3)result;
        }

        public string Name { get; }
        public Func<T1, T2, T3> Func { get; }
        public IEnumerable<Argument> Arguments { get; }
        public Type ReturnType { get; }
    }
}