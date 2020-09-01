using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using SimpleScript.Binding;
using Xunit;
using Xunit.Abstractions;
using Zafiro.Core.Patterns;

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
        public void Test(string input, string expected)
        {
            var sut = new Binder(new BindingContext(new List<IFunction>()
            {
                new Function("Call"),
            }));

            var parser = new EnhancedParser();

            var result = parser
                .Parse(input)
                .MapError(pr => new ErrorList(pr.Message)).MapSuccess(enhancedScript => sut.Bind(enhancedScript))
                .MapSuccess(script =>
                {
                    var visitor = new StringifyVisitor();
                    script.Accept(visitor);
                    return visitor.ToString();
                })
                .Handle(Flatten);
                
            testOutputHelper.WriteLine(result);
            testOutputHelper.WriteLine("_________________");
            testOutputHelper.WriteLine(expected);

            result.Should().Be(expected);
        }

        public static IEnumerable<object[]> Data()
        {
            //yield return new object[] { File.ReadAllText("TestData\\Inputs\\File1.txt"), File.ReadAllText("TestData\\Expectations\\File1.txt") };
            //yield return new object[] { File.ReadAllText("TestData\\Inputs\\File3.txt"), File.ReadAllText("TestData\\Expectations\\File3.txt") };
            //yield return new object[] { File.ReadAllText("TestData\\Inputs\\File4.txt"), File.ReadAllText("TestData\\Expectations\\File4.txt") };
            //yield return new object[] { File.ReadAllText("TestData\\Inputs\\File5.txt"), File.ReadAllText("TestData\\Expectations\\File5.txt") };
            yield return new object[] { File.ReadAllText("TestData\\Inputs\\File6.txt"), File.ReadAllText("TestData\\Expectations\\File6.txt") };
        }

        private string Flatten(ErrorList list)
        {
            return string.Join(",", list);
        }
    }
}