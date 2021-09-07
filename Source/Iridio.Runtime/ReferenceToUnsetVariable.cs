using CSharpFunctionalExtensions;
using Iridio.Core;

namespace Iridio.Runtime
{
    public class ReferenceToUnsetVariable : RunError
    {
        public string[] VariableNames { get; }

        public ReferenceToUnsetVariable(Maybe<Position> position, params string[] variableNames) : base(position)
        {
            VariableNames = variableNames;
        }

        public override string ToString()
        {
            return $"Usage of unset variable: {string.Join(", ", VariableNames)}";
        }
    }
}