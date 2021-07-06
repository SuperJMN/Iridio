using System.Collections.Generic;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentAssertions.CSharpFunctionalExtensions;
using Iridio.Runtime;
using Xunit;

namespace Iridio.Tests
{
    public class StringEvaluatorTests
    {
        [Fact]
        public void All_variables_should_be_found()
        {
            var sut = new StringEvaluator();
            var dict = new Dictionary<string, object>
            {
                {"replaced", "mate"},
                {"replaced_between_braces", "boy!"},
                {"non_replaced", "will not appear on resulting string"},
            };

            var actual = sut.Evaluate("Hello {replaced}, {{non_replaced}}, {{{replaced_between_braces}}}", dict);
            var expected = Result.Success<string, RuntimeErrors>("Hello mate, {non_replaced}, {boy!}");
            actual.Should().BeSuccess()
                .And
                .Subject.Value.Should().Be("Hello mate, {non_replaced}, {boy!}");
        }

        [Fact]
        public void Variable_not_found()
        {
            var sut = new StringEvaluator();
            var dict = new Dictionary<string, object>();

            var actual = sut.Evaluate("This will {fail}", dict);
            actual.Should().BeFailure();
        }
    }
}