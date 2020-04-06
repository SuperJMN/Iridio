using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace SimpleScript.Tests
{
    public class RunTests
    {
        [Fact]
        public async Task MyTest()
        {
            MethodInfo inf = typeof(IntTask).GetMethod("Execute");
            var intTask = new IntTask();
            Delegate d = inf.CreateDelegate(intTask);
            var ret =  await (Task<object>) d.Method.Invoke(intTask, new object[] {123});
        }

        [Fact]
        public async Task Run()
        {
            IEnumerable<Function> funcs = new[]{ new Function(typeof(IntTask)), new Function(typeof(StringTask)), };
            var runner = new ScriptRunner(funcs);
            var dictionary = new Dictionary<string, object>();
            var taskFactory = new ScriptFactory(new ScriptParser(), runner, funcs);
            var script = taskFactory.Load("a = IntTask(1);\nb = \"Johnny was a good man\";\nStringTask(\"{b}\");");
            await script.Run(dictionary);
        }
    }
}