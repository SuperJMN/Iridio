using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iridio.Binding;
using Iridio.Parsing.Model;

namespace Iridio.Common
{
    public interface IFunction : INamed
    {
        Task<object> Invoke(object[] parameters);
        IEnumerable<Parameter> Arguments { get; }
        Type ReturnType { get; }
    }
}