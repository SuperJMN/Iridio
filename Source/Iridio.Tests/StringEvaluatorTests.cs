using System.Collections.Generic;
using FluentAssertions;
using Iridio.Runtime;
using Iridio.Runtime.ReturnValues;
using Optional;
using Xunit;
using Zafiro.Core.Patterns.Either;

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
            var expected = Either.Success<RuntimeErrors, string>("Hello mate, {non_replaced}, {boy!}");
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Variable_not_found()
        {
            var sut = new StringEvaluator();
            var dict = new Dictionary<string, object>();

            var actual = sut.Evaluate("This will {fail}", dict);
            var expected = Either.Error<RuntimeErrors, string>(new RuntimeErrors(new ReferenceToUnsetVariable("fail")));
            actual.Should().BeEquivalentTo(expected, options => options
                .ComparingByMembers(typeof(Either<,>))
                .ComparingByMembers(typeof(Option<RuntimeErrors>)));
        }
    }
}