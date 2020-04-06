using System.Threading.Tasks;

namespace SimpleScript.Tests
{
    public class StringTask
    {
        public async Task<object> Execute(string b)
        {
            return $"Hello{b}";
        }
    }
}