using Iridio.Core;

namespace Iridio.Binding
{
    public class ProcedureSymbol
    {
        public ProcedureSymbol(string name, Position position)
        {
            Name = name;
            Position = position;
        }

        public string Name { get; }
        public Position Position { get; }
    }
}