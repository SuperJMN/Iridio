using System.Collections.Generic;

namespace SimpleScript
{
    public interface IMetadataProvider
    {
        List<string> GetMetadata(Script syntax);
    }
}