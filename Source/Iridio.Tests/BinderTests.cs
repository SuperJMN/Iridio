using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing;
using Optional;
using Xunit;
using Zafiro.Core.Patterns.Either;

namespace Iridio.Tests
{
    public class BinderTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Test(string testName, string input, Either<Errors, string> expected)
        {
            var sut = CreateSut();
            Bind(input, sut)
                .Should().BeEquivalentTo(expected, CompareOptions);
        }

        [Fact]
        public void Unbound_procedure_should_not_be_unknown()
        {
            var sut = CreateSut();

            Bind(File.ReadAllText("TestData\\Inputs\\Procedure_with_undeclared_function_call.txt"), sut)
                .WhenLeft(
                    errors => errors.Should().NotContain(new Error(ErrorKind.UndeclaredFunctionOrProcedure, "Function")));
        }

        private static Binder CreateSut()
        {
            return new Binder(new BindingContext(new List<IFunction>()
            {
                new Function("Call"),
            }));
        }

        private static
            Func<EquivalencyAssertionOptions<Either<Errors, string>>,
                EquivalencyAssertionOptions<Either<Errors, string>>> CompareOptions
        {
            get
            {
                return opts =>
                {
                    return opts
                        .Using<Error>(config => config.Subject.ErrorKind.Should().Be(config.Expectation.ErrorKind))
                        .WhenTypeIs<Error>()
                        .ComparingByMembers<Option<Errors>>()
                        .ComparingByMembers<Either<Errors, string>>();
                };
            }
        }

        private static Either<Errors, string> Bind(string input, Binder sut)
        {
            var parser = new Parser();

            var result = parser
                .Parse(input)
                .MapLeft(pr => new Errors(ErrorKind.UnableToParse))
                .MapRight(enhancedScript => sut.Bind(enhancedScript))
                .MapRight(script => script.AsString());

            return result;
        }

        public static IEnumerable<object[]> Data()
        {
            yield return new object[]
            {
                "File1", File.ReadAllText("TestData\\Inputs\\File1.txt"),
                File.ReadAllText("TestData\\Expectations\\File1.txt")
            };
            yield return new object[]
            {
                "File3", File.ReadAllText("TestData\\Inputs\\File3.txt"),
                File.ReadAllText("TestData\\Expectations\\File3.txt")
            };
            yield return new object[]
            {
                "File4", File.ReadAllText("TestData\\Inputs\\File4.txt"),
                File.ReadAllText("TestData\\Expectations\\File4.txt")
            };
            yield return new object[]
            {
                "File5", File.ReadAllText("TestData\\Inputs\\File5.txt"),
                File.ReadAllText("TestData\\Expectations\\File5.txt")
            };
            yield return new object[]
            {
                "File6", File.ReadAllText("TestData\\Inputs\\File6.txt"),
                File.ReadAllText("TestData\\Expectations\\File6.txt")
            };
            yield return new object[]
                {"File7", File.ReadAllText("TestData\\Inputs\\File7.txt"), new Errors(ErrorKind.UndefinedMainFunction)};
            yield return new object[]
            {
                "Usage_of_unset_variable_in_string",
                File.ReadAllText("TestData\\Inputs\\Usage_of_unset_variable_in_string.txt"),
                new Errors(ErrorKind.ReferenceToUninitializedVariable)
            };
            yield return new object[]
            {
                "Usage_of_unset_variable", File.ReadAllText("TestData\\Inputs\\Usage_of_unset_variable.txt"),
                new Errors(ErrorKind.ReferenceToUninitializedVariable)
            };
        }
    }
}