using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Iridio.Common
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

        public static Errors Concat(Errors error, Errors anotherError)
        {
            return new Errors(error.Concat(anotherError));
        }

        public static string Join(Errors list)
        {
            return string.Join(Environment.NewLine, list);
        }
    }
}