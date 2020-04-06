using System;
using System.Threading.Tasks;

namespace SimpleScript.Tests
{
    [Metadata("Dependency", "value")]
    [Metadata("Dependency", "value")]
    public class IntTask
    {
        public async Task<object> Execute(int b)
        {
            return b;
        }
    }
}