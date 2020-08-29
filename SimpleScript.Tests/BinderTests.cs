using System.Collections.Generic;
using FluentAssertions;
using SimpleScript.Binding;
using Xunit;

namespace SimpleScript.Tests
{
    public class BinderTests
    {
        [Fact]
        public void Test()
        {
            var sut = new Binder(new BindingContext(new List<IFunction>()));
            var parser = new EnhancedParser();
            var result = 
                parser.Parse("Main { }")
                .MapSuccess(script =>
                {
                    return sut.Bind(script).MapError(list => list)
                        .MapSuccess(boundScript => boundScript.ToString())
                        .Handle(s => string.Join(",", s));
                })
                .MapError(error => "Parse error")
                .Handle(s => "");

            var expected = "something";
            result.Should().Be(expected);
        }
    }
}