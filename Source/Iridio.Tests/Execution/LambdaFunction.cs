﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iridio.Common;
using Iridio.Parsing.Model;

namespace Iridio.Tests.Execution
{
    public class LambdaFunction<T> : IFunction
    {
        public LambdaFunction(string name, Func<T> func)
        {
            Name = name;
            Func = func;
        }

        public async Task<object> Invoke(object[] parameters)
        {
            var result = Func.DynamicInvoke(parameters);
            return (T)result;
        }

        public IEnumerable<Parameter> Parameters { get; }
        public Type ReturnType { get; }

        public string Name { get; }
        public Func<T> Func { get; }
    }

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
        private Func<T1, T2, T3> Func { get; }
        public IEnumerable<Parameter> Parameters { get; }
        public Type ReturnType { get; }
    }
}