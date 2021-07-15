using System.Threading.Tasks;

namespace Iridio.Tests.Execution
{
    public class IntTask
    {
        public async Task<object> Execute(int b)
        {
            return b;
        }
    }
}