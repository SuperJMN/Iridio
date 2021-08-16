using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Runtime
{
    public class MainProcedureNotFound : RunError
    {
        public override IReadOnlyCollection<Error> Errors { get; }

        public override string ToString()
        {
            return "Cannot find 'Main' procedure";
        }

        public MainProcedureNotFound() : base(new Position(0, 0))
        {
        }
    }
}