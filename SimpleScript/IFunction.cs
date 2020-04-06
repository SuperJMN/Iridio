using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleScript
{
    public interface IFunction
    {
        Task<object> Invoke(object[] parameters);
        string Name { get; }
        IEnumerable<(string Key, object Value)> Metadata { get; }
    }
}