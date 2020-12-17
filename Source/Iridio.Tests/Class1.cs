using System.Collections.Generic;
using System.IO;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing;
using Xunit;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Tests
{
    public class Class1
    {
        [Fact]
        public void Test()
        {
            var sut = CreateSut();
            var result = Bind(File.ReadAllText("TestData\\Inputs\\Usage_of_unset_variable.txt"), sut);
        }

        private static Either<Errors, string> Bind(string input, Binder sut)
        {
            var parser = new Parser();

            var result = parser
                .Parse(input)
                .MapLeft(pr => new Errors(ErrorKind.UnableToParse))
                .MapRight(enhancedScript => sut.Bind(enhancedScript))
                .MapRight(script => "FIXME");

            return result;
        }


        private static Binder CreateSut()
        {
            return new Binder(new List<IFunctionDeclaration> { new FunctionDeclaration("Call")});
        }
    }
}