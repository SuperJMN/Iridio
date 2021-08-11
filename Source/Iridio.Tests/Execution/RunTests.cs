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
    }
}