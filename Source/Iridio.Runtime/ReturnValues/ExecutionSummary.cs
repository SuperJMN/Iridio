using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Iridio.Runtime.ReturnValues
{
    public class ExecutionSummary
    {
        private readonly IDictionary<string, object> variables;

        public ExecutionSummary(IDictionary<string, object> variables)
        {
            this.variables = variables;
        }

        public IDictionary<string, object> Variables => new ReadOnlyDictionary<string, object>(variables);
    }
}