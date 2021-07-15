using System;
using System.Reflection;
using System.Threading.Tasks;
using Iridio.Common.Utils;
using Xunit;

namespace Iridio.Tests.Execution
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

        //[Fact]
        //public async Task Run()
        //{
        //    IEnumerable<ProcedureDeclaration> funcs = new[] { new ProcedureDeclaration(typeof(IntTask)), new ProcedureDeclaration(typeof(StringTask)), };
        //    var runner = new Runner(funcs);
        //    var compiler = new Compiler(new Parser(), new FileSystemOperations());
        //    var script = compiler.Compile("Root.txt");
        //    var dictionary = new Dictionary<string, object>();
        //    await runner.Run(script, dictionary);
        //}
    }
}