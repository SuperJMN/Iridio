using System.Threading.Tasks;
using FluentAssertions;
using Iridio.Common.Utils;
using Iridio.Tests.Execution;
using Xunit;

namespace Iridio.Tests.Extra
{
    public class DynamicInvocationTests
    {
        [Fact]
        public async Task Invoking_IntTask_with_param_returns_same_value()
        {
            var p = new IntTask();
            var result = await p.InvokeTask("Execute", 1);
            result.Should().Be(1);
        }
    }
}