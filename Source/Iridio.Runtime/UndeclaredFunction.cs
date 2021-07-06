using System.Collections.Generic;

namespace Iridio.Runtime
{
    public class UndeclaredFunction : RunError
    {
        public string FunctionName { get; }

        public UndeclaredFunction(string functionName)
        {
            FunctionName = functionName;
        }

        public override IEnumerable<string> Items => new[] { ToString() };

        public override string ToString()
        {
            return $"Function {FunctionName} is not declared";
        }
    }
}