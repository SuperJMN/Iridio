using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleScript.Parsing.Model;

namespace SimpleScript
{
    public interface IFunction
    {
        Task<object> Invoke(object[] parameters);
        string Name { get; }
        IEnumerable<Argument> Arguments { get; }
        Type ReturnType { get; }
    }
}