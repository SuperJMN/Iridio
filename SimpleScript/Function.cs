using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleScript
{
    public class Function : IFunction
    {
        private object instance;
        private MethodInfo method;

        public Function(Type type)
        {
            Name = type.Name;
            instance = Activator.CreateInstance(type);
            method = type.GetMethod("Execute");
        }

        public Task<object> Invoke(object[] parameters)
        {
            return method.InvokeTask(instance, parameters);
        }

        public string Name { get; }
    }
}