using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing;
using Iridio.Runtime;
using Xunit;

namespace Iridio.Tests.Core
{
    public class Success : IridioTestBase
    {
        [Theory]
        [ClassData(typeof(Data))]
        public async Task References_have_correct_values_after_execution(string source, IDictionary<string, object> expectations)
        {
            var sut = CreateSut();

            var result = await sut.Run(SourceCode.FromString(source), new Dictionary<string, object>());
            result
                .Should().BeSuccess()
                .And.Subject.Value.Variables.Should().Contain(expectations);
        }

        [Fact]
        public async Task Run_twice_should_produce_the_same_outcome()
        {
            var sut = CreateSut();

            var firstExecution = await sut.Run(SourceCode.FromString("Main { }"), new Dictionary<string, object>());
            var secondExecution = await sut.Run(SourceCode.FromString("Main { }"), new Dictionary<string, object>());
            firstExecution
                .Should().BeSuccess()
                .And.Subject.Value.Should().BeEquivalentTo(secondExecution.Value);
        }


        [Fact]
        public async Task Messages_are_pushed_from_script()
        {
            var externalFunctions = Array.Empty<IFunction>();
            var sut = new Runtime.Iridio(new SourceCodeCompiler(new Binder(externalFunctions), new Parser()), new Interpreter(externalFunctions));
            var message = Maybe<string>.None;
            sut.Messages.Subscribe(s => message = Maybe<string>.From(s));

            var result = await sut.Run(SourceCode.FromString(GetMain("'My message'")), new Dictionary<string, object>());

            result.Should().BeSuccess();
            message.HasValue.Should().BeTrue();
            message.Value.Should().Be("My message");
        }

        private class Data : TheoryData<string, IDictionary<string, object>>
        {
            public Data()
            {
                AddOneLinerCase("Sum(1, 3);", 4);
                AddOneLinerCase("true;", true);
                AddOneLinerCase("Sum(Sum(2,5), 3);", 10);
                AddOneLinerCase("5;", 5);
                AddOneLinerCase("5d;", 5d);
                AddOneLinerCase("1+2;", 3);
                AddOneLinerCase("5-7;", -2);
                AddOneLinerCase("3*2;", 6);
                AddOneLinerCase("6/2;", 3);
                AddOneLinerCase("5d/2;", 2.5D);
                AddOneLinerCase("12/2*3;", 18);
                AddOneLinerCase("12/(2*3);", 2);
                AddOneLinerCase("12==4;", false);
                AddOneLinerCase("12!=4;", true);
                AddOneLinerCase("4==4;", true);
                AddOneLinerCase("true;", true);
                AddOneLinerCase("true || false;", true);
                AddOneLinerCase("true && false;", false);
                AddOneLinerCase("!true;", false);
                AddOneLinerCase("!false;", true);
                AddCase("b = 1; if (b == 1)  { a = 2; }  else  { a = 6; }", ("a", 2));
                AddCase("b = 1; if (b != 1)  { a = 2; }  else  { a = 6; }", ("a", 6));
                AddCase("b=\"Hello\"; a = \"{b} world!\";", ("a", "Hello world!"));
                Add("Main {  Proc1(); } Proc1 { a = \"OK\"; }", new Dictionary<string, object> { ["a"] = "OK" });
            }

            private void AddCase(string code, params (string, object)[] expectedValues)
            {
                Add(GetMain(code), expectedValues.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2));
            }

            private void AddOneLinerCase(string code, object value)
            {
                Add(GetMain("a = " + code), new Dictionary<string, object> { ["a"] = value });
            }
        }

        private static string GetMain(string code)
        {
            return "Main { " + code + " }";
        }
    }
}