using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using SimpleScript.Binding;
using Xunit;
using Xunit.Abstractions;
using Zafiro.Core.Patterns;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Tests
{
    public class BinderTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public BinderTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Test(string input, Either<Errors, string> expected)
        {
            var sut = new Binder(new BindingContext(new List<IFunction>()
            {
                new Function("Call"),
            }));

            var parser = new EnhancedParser();

            var result = parser
                .Parse(input)
                .MapLeft(pr => new Errors(ErrorKind.UnableToParse))
                .MapRight(enhancedScript => sut.Bind(enhancedScript))
                .MapRight(script => script.AsString());


            result.Should().BeEquivalentTo(expected, opts =>
            {
                return opts
                    .Using<Error>(config => config.Subject.ErrorKind.Should().Be(config.Expectation.ErrorKind)).WhenTypeIs<Error>()
                    .ComparingByMembers<Option<Errors>>()
                    .ComparingByMembers<Either<Errors, string>>(); ;
            });
        }

        public static IEnumerable<object[]> Data()
        {
            yield return new object[] { File.ReadAllText("TestData\\Inputs\\File1.txt"), File.ReadAllText("TestData\\Expectations\\File1.txt") };
            yield return new object[] { File.ReadAllText("TestData\\Inputs\\File3.txt"), File.ReadAllText("TestData\\Expectations\\File3.txt") };
            yield return new object[] { File.ReadAllText("TestData\\Inputs\\File4.txt"), File.ReadAllText("TestData\\Expectations\\File4.txt") };
            yield return new object[] { File.ReadAllText("TestData\\Inputs\\File5.txt"), File.ReadAllText("TestData\\Expectations\\File5.txt") };
            yield return new object[] { File.ReadAllText("TestData\\Inputs\\File6.txt"), File.ReadAllText("TestData\\Expectations\\File6.txt") };
            yield return new object[] { File.ReadAllText("TestData\\Inputs\\File7.txt"), new Errors(ErrorKind.UndefinedMainFunction) };
        }
    }
}