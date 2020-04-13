using System.Threading.Tasks;

namespace SimpleScript.Tests
{
    public class IntTask
    {
        public async Task<object> Execute(int b)
        {
            return b;
        }
    }
}