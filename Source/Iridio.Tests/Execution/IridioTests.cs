using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Common;
using Iridio.Runtime;
using Xunit;

namespace Iridio.Tests.Execution
{
    public class IridioTests
    {
        [Theory]
        [ClassData(typeof(Data))]
        public async Task Test(string source, IDictionary<string, object> expectations)
        {
            var sourceFileName = "script.rdo";
            var sut = new IridioShell(new List<IFunction>
            {
                new LambdaFunction<int, int, int>("Sum", (x, y) => x + y)
            }, new MockFileSystem(new Dictionary<string, MockFileData> { { sourceFileName, new MockFileData(source) } }));

            var result = await sut.Run(sourceFileName);
            result
                .Should().BeSuccess()
                .And.Subject.Value.Variables.Should().Contain(expectations);
        }

        private class Data : TheoryData<string, IDictionary<string, object>>
        {
            public Data()
            {
                Add("Main { a = Sum(1, 3); }", new Dictionary<string, object> { ["a"] = 4 });
                Add("Main { a = true; }", new Dictionary<string, object> { ["a"] = true  });
                Add("Main { a = Sum(Sum(2,5), 3); }", new Dictionary<string, object> { ["a"] = 10 });
                Add("Main { a = 3; b = 5; if (b > a) { c = true; } }", new Dictionary<string, object> { ["c"] = true  });
            }
        }
    }
}