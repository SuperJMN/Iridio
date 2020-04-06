using System;

namespace SimpleScript.Tests
{
    public class InstanceCreator : IInstanceBuilder
    {
        public object Build(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}