using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Runtime.ReturnValues;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Runtime
{
    public interface IScriptRunner
    {
        Task<Either<RuntimeErrors, Success>> Run(Script script, IDictionary<string, object> variables);
        IObservable<string> Messages { get; }
    }
}