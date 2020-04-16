using System;

namespace SimpleScript
{
    public class Argument
    {
        public Argument(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public Type Type { get; }
        public string Name { get; }
    }
}