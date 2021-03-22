using System.Collections.Generic;

namespace Iridio.Runtime
{
    public class ReferenceToUnsetVariable : RunError
    {
        public string VariableName { get; }

        public ReferenceToUnsetVariable(string variableName)
        {
            VariableName = variableName;
        }

        public override IEnumerable<string> Items => new[] {ToString()};

        public override string ToString()
        {
            return $"Usage of unset variable '{VariableName}'";
        }
    }
}