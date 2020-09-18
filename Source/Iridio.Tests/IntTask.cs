using System.Threading.Tasks;

namespace Iridio.Tests
{
    public class IntTask
    {
        public async Task<object> Execute(int b)
        {
            return b;
        }
    }
}