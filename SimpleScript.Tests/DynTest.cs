using System.Threading.Tasks;
using SimpleScript;
using Xunit;

namespace Tests
{
    public class DynTest
    {
        [Fact]
        public async Task Test()
        {
            var p = new IntTask();
            var result = await p.ExecuteTask("Execute", 1);
        }
    }
}