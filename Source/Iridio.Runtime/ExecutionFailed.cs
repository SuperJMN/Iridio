namespace Iridio.Runtime
{
    internal class ExecutionFailed : RuntimeError
    {
        public RunError RunError { get; }

        public ExecutionFailed(RunError runError)
        {
            RunError = runError;
        }

        public override string ToString()
        {
            return RunError.ToString();
        }
    }
}