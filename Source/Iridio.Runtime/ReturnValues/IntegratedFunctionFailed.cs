using System;
using System.Collections.Generic;
using Iridio.Common;

namespace Iridio.Runtime.ReturnValues
{
    public class IntegratedFunctionFailed : RuntimeError
    {
        public IFunction Function { get; }
        public Exception Exception { get; }

        public IntegratedFunctionFailed(IFunction function, Exception exception)
        {
            Function = function;
            Exception = exception;
        }

        public override string ToString()
        {
            return $"Function {Function.Name} threw an exception: '{Exception.Message}'";
        }

        public override IEnumerable<string> Items => new[] {ToString()};
    }
}