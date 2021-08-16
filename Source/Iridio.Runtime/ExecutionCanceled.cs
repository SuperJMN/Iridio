using Iridio.Core;

namespace Iridio.Runtime
{
    public class ExecutionCanceled : RunError
    {
        public string Message { get; }

        public ExecutionCanceled(string message, Position position) : base(position)
        {
            Message = message;
        }

        public override string ToString()
        {
            return "Execution canceled";
        }
    }
}