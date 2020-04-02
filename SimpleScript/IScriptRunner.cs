using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleScript
{
    public interface IScriptRunner
    {
        Task Run(string source, IDictionary<string, object> variables = null);
    }
}