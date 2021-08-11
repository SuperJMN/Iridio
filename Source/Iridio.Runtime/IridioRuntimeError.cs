namespace Iridio.Runtime
{
    public class IridioRuntimeError : IridioError
    {
        public RunError Error { get; }

        public IridioRuntimeError(RunError error)
        {
            Error = error;
        }

        public override string ToString()
        {
            return $"Runtime error: {Error}";
        }
    }
}