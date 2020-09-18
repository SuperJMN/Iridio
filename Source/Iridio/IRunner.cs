using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iridio
{
    public interface IRunner
    {
        Task Run(Script syntax, IDictionary<string, object> variables = null);
        IObservable<string> Messages { get; }
    }
}