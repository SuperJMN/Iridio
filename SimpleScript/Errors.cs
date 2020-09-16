using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleScript
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
    }
}