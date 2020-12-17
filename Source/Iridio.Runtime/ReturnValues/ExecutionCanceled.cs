using System.Collections.Generic;

namespace Iridio.Runtime.ReturnValues
{
    public class ExecutionCanceled : RuntimeError
    {
        public string Message { get; }

        public ExecutionCanceled(string message)
        {
            Message = message;
        }

        public override IEnumerable<string> Items => new[]{ this.ToString() };

        public override string ToString()
        {
            return "Execution canceled";
        }
    }
}