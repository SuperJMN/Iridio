using System.Collections.Generic;

namespace Iridio.Core
{
    public interface IErrorList
    {
        public IReadOnlyCollection<ErrorItem> Errors { get; }
    }
}