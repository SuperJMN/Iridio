using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleScript.Parsing.Model;

namespace SimpleScript.Tests
{
    public class Function : IFunction
    {
        public Function(string name)
        {
            Name = name;
        }

        public Task<object> Invoke(object[] parameters)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
        public IEnumerable<Argument> Arguments { get; }
        public Type ReturnType { get; }
    }
}