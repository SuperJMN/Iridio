using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Iridio.Common
{
    public class Errors : Collection<BinderError>
    {
        public Errors(params BinderError[] errors) : base(errors)
        {
        }

        public Errors(IEnumerable<BinderError> items) : base(items.ToList())
        {
        }

        public Errors()
        {
        }

        public override string ToString()
        {
            return string.Join(";", this);
        }
    }
}