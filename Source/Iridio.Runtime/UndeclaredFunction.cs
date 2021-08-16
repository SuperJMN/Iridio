using System.Collections.Generic;
using Iridio.Core;

namespace Iridio.Runtime
{
    public class UndeclaredFunction : RunError
    {
        public string FunctionName { get; }
        public Position Position { get; }

        public UndeclaredFunction(string functionName, Position position) : base(position)
        {
            FunctionName = functionName;
            Position = position;
        }

        public override IReadOnlyCollection<Error> Errors =>
            new List<Error> { new($"Function {FunctionName} is not declared", Position) }.AsReadOnly();
    }
}