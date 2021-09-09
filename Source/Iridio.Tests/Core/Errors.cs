using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Iridio.Parsing;
using Iridio.Runtime;
using Xunit;

namespace Iridio.Tests.Core
{
    public class Errors : IridioCoreTestsBase
    {
        [Fact]
        public async Task No_main()
        {
            var result = await Run(SourceCode.FromString("Procedure { }"));
            CheckErrors(result, "Cannot find 'Main'");
        }

        [Fact]
        public async Task Undefined_reference()
        {
            var result = await Run(SourceCode.FromString(@"Main { 
    a = b;
}"));
            CheckErrors(result, "unset");
        }

        [Fact]
        public async Task Undefined_reference_in_string()
        {
            var sut = CreateSut();
            var result = await sut.Run(SourceCode.FromString(@"Main { 
    a = ""{b}"";
}"));
            CheckErrors(result, "unset");
        }

        [Fact]
        public async Task Function_and_procedure_conflict()
        {
            var result = await Run(SourceCode.FromString("Main \r\n{  \r\n} \r\nSum \r\n{ \r\n}"));
            CheckErrors(result, "conflict");
        }

        [Fact]
        public async Task Function_call_exception()
        {
            var result = await Run(SourceCode.FromString("Main \r\n{  \r\n    Throw();\r\n}"));
            CheckErrors(result, "exception");
        }

        [Fact]
        public async Task Cancellation()
        {
            var result = await Run(SourceCode.FromString("Main \r\n{  \r\n    Cancel();\r\n}"));
            CheckErrors(result, "cancel");
        }

        [Fact]
        public async Task Undeclared()
        {
            var result = await Run(SourceCode.FromString("Main \r\n{  \r\n    Undeclared();\r\n}"));
            CheckErrors(result, "undeclared");
        }

        [Fact]
        public async Task Duplicate_definition()
        {
            var result = await Run(SourceCode.FromString(
                @"Main 
{ 
}

Main 
{
}"));
            CheckErrors(result, "already");
        }

        [Fact]
        public async Task Unset_variable()
        {
            var result = await Run(SourceCode.FromString(
                @"Main 
            {
                b = a;
            }"));
            CheckErrors(result, "unset");
        }

        private static void CheckErrors(Result<ExecutionSummary, IridioError> result, string expectation)
        {
            var errorString = result.Match(x => "{no errors}", x => string.Join("\n ", x.Errors.Select(item => item.ToString())));

            errorString.Should()
                .ContainEquivalentOf(expectation);
        }
    }
}