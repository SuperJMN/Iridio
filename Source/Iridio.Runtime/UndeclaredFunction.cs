using Iridio.Core;

namespace Iridio.Runtime
{
    public class UndeclaredFunction : RunError
    {
        public string Name { get; }

        public UndeclaredFunction(string name, Position position) : base(position)
        {
            Name = name;
        }
    }
}