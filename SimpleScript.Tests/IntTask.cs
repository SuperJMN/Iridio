using System.Threading.Tasks;

namespace Tests
{
    public class IntTask
    {
        public async Task<object> Execute(int b)
        {
            return b;
        }
    }
}