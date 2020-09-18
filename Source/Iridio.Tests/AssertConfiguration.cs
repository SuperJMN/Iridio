using System;
using FluentAssertions.Equivalency;
using Iridio.Common;

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