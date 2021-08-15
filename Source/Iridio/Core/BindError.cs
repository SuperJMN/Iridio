using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Iridio.Binding;

namespace Iridio.Core
{
    public class BindError : CompilerError
    {
        public BinderErrors BinderErrors { get; }

        public BindError(BinderErrors binderErrors)
        {
            BinderErrors = binderErrors;
        }

        public override string ToString()
        {
            return string.Join(";", BinderErrors.Select(x => x.ToString()));
        }

        public override IReadOnlyCollection<RichError> Errors => new ReadOnlyCollection<RichError>(new List<RichError>());
    }
}