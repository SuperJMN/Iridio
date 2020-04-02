using System;
using SimpleScript;

namespace Tests
{
    public class InstanceCreator : IInstanceBuilder
    {
        public object Build(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}