namespace SimpleScript.Tests
{
    public class TestFileOperations : IFileOperations
    {
        private readonly string source;

        public TestFileOperations(string source)
        {
            this.source = source;
        }

        public string ReadAllText(string path)
        {
            return source;
        }
    }
}