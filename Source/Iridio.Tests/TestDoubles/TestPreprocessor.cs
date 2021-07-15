using System.Linq;
using Iridio.Parsing;
using Iridio.Preprocessing;
using Zafiro.Core.Mixins;

namespace Iridio.Tests.Execution
{
    public class TestPreprocessor : IPreprocessor
    {
        private readonly string source;

        public TestPreprocessor(string source)
        {
            this.source = source;
        }

        public PreprocessedSource Process(string path)
        {
            return new PreprocessedSource(
                source.Lines().Select((s, i) => new TaggedLine(s, "fake", i + 1)).ToList());
        }
    }
}