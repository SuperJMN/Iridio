using System;
using FluentAssertions.Equivalency;

namespace Iridio.Tests
{
    public static class AssertConfiguration 
    {
        public static Func<EquivalencyAssertionOptions<Error>, EquivalencyAssertionOptions<Error>> ForErrors
        {
            get { return options => options.Excluding(error => error.AdditionalData); }
        }
    }
}