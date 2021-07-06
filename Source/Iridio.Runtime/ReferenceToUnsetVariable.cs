using System.Collections.Generic;

namespace Iridio.Runtime
{
    public class ReferenceToUnsetVariable : RunError
    {
        public string[] VariableNames { get; }

        public ReferenceToUnsetVariable(params string[] variableNames)
        {
            VariableNames = variableNames;
        }

        public override IEnumerable<string> Items => new[] {ToString()};

        public override string ToString()
        {
            return $"Usage of unset variable: {string.Join(", ", VariableNames)}";
        }
    }
}