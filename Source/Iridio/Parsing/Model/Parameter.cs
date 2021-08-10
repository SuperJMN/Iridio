using System;

namespace Iridio.Parsing.Model
{
    public class Parameter
    {
        public Parameter(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public Type Type { get; }
        public string Name { get; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}