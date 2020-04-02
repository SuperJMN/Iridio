using System;

namespace SimpleScript
{
    public interface IInstanceBuilder
    {
        object Build(Type type);
    }
}