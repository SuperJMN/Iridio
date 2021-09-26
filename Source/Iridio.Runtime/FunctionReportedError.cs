using CSharpFunctionalExtensions;
using Iridio.Common;
using Iridio.Core;

namespace Iridio.Runtime
{
    public class FunctionReportedError : RunError
    {
        public IFunction Function { get; }
        public string ErrorMessage { get; }

        public FunctionReportedError(IFunction function, string errorMessage, Maybe<Position> position) : base(position)
        {
            Function = function;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"Function {Function} reported an error: {ErrorMessage}";
        }
    }
}