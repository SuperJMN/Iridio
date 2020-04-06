using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleScript.Tests
{
    public class Invoker : ITaskCaller
    {
        private readonly Dictionary<string, (object instance, MethodInfo method)> methods;

        public Invoker(IEnumerable<Type> typeUniverse)
        {
            var query = from type in typeUniverse
                let instance = Activator.CreateInstance(type)
                let method = type.GetMethod("Execute")
                where method != null
                let del = method.CreateDelegate(instance)
                let callSite = (instance, method)
                select new {Name = type.Name, CallSite = callSite};

            methods = query.ToDictionary(arg => arg.Name, arg => arg.CallSite);
        }

        public Task<object> Invoke(string funcName, object[] parameters)
        {
            var tuple = methods[funcName];
            return tuple.method.InvokeTask(tuple.instance, parameters);
        }
    }
}