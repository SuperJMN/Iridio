using System;
using System.Collections.Generic;
using Iridio.Common;
using Iridio.Core;

namespace Iridio.Runtime
{
    public class IntegratedFunctionFailed : RunError
    {
        public IFunction Function { get; }
        public Exception Exception { get; }
        public Position Position { get; }

        public IntegratedFunctionFailed(IFunction function, Exception exception, Position position) : base(position)
        {
            Function = function;
            Exception = exception;
            Position = position;
        }

        public override string ToString()
        {
            return $"Function {Function.Name} threw an exception: '{Exception.Message}'";
        }

        public override IReadOnlyCollection<Error> Errors => new List<Error>
        {
            new($"Function {Function.Name} threw an exception: '{Exception.Message}'", Position)
        };
    }
}