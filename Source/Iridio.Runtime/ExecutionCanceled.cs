using System.Collections.Generic;

namespace Iridio.Runtime
{
    public class ExecutionCanceled : RunError
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