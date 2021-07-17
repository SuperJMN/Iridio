using System.Collections.Generic;

namespace Iridio.Runtime
{
    public abstract class RunError
    {
        public abstract IEnumerable<string> Items { get; }
    }

    public class MainProcedureNotFound : RunError
    {
        public override IEnumerable<string> Items => new[] {ToString()};

        public override string ToString()
        {
            return "Cannot find 'Main' procedure";
        }
    }
}