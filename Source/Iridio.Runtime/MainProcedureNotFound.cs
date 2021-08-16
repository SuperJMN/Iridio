using Iridio.Core;

namespace Iridio.Runtime
{
    public class MainProcedureNotFound : RunError
    {
        public override string ToString()
        {
            return "Cannot find 'Main' procedure";
        }

        public MainProcedureNotFound() : base(new Position(0, 0))
        {
        }
    }
}