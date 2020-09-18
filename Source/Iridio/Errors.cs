using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Iridio
{
    public class Errors : Collection<Error>
    {
        public Errors(params Error[] errors) : base(errors)
        {
        }

        public Errors(IEnumerable<Error> items) : base(items.ToList())
        {
        }

        public Errors()
        {
        }

        public static Errors Concat(Errors another, Errors errors)
        {
            return new Errors(Enumerable.Concat(another, errors));
        }

        public static string Join(Errors list)
        {
            return string.Join(Environment.NewLine, list);
        }
    }
}