using System.Collections.Generic;
using System.IO;

namespace SimpleScript
{
    public static class StringUtils
    {
        public static IEnumerable<string> Lines(this string s)
        {
            using var tr = new StringReader(s);
            while (tr.ReadLine() is { } l)
                yield return l;
        }
    }
}