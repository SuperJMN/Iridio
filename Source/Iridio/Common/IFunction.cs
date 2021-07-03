using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iridio.Binding;
using Iridio.Parsing.Model;

namespace Iridio.Common
{
    public interface IFunction : IFunctionDeclaration
    {
        Task<object> Invoke(object[] parameters);
        IEnumerable<Argument> Arguments { get; }
        Type ReturnType { get; }
    }
}