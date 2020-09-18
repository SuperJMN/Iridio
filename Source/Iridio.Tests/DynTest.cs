using System.Threading.Tasks;
using Xunit;

namespace SimpleScript.Tests
{
    public class DynTest
    {
        [Fact]
        public async Task Test()
        {
            var p = new IntTask();
            var result = await p.InvokeTask("Execute", 1);
        }
    }
}