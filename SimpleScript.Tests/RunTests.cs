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
            var runner = new Runner(funcs);
            var compiler = new Compiler(new TestFileOperations(""), new Parser());
            var source = @"a = IntTask(1);
b = ""Johnny was a good man"";
StringTask(""{b}"");
!""c:\myscript.txt"";";

            var script = compiler.Compile(source);
            var dictionary = new Dictionary<string, object>();
            await runner.Run(script, dictionary);
        }
    }
}