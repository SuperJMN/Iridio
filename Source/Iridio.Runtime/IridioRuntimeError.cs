namespace Iridio.Runtime
{
    public class IridioRuntimeError : IridioError
    {
        public RunError Error { get; }

        public IridioRuntimeError(RunError error)
        {
            Error = error;
        }
    }
}