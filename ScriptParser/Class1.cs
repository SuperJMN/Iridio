using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using SimpleScript;
using Xunit;

namespace ScriptParser
{
    public class DynTest
    {
        [Fact]
        public async Task Test()
        {
            var p = new MyTask();
            var result = await p.ExecuteTask("Execute", 1);
        }
    }

    public class MyTask
    {
        public async Task<object> Execute(int b)
        {
            return b;
        }
    }

    public class InstanceCreator : IInstanceBuilder
    {
        public object Build(Type type, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}