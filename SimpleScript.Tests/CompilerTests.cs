using System.Linq;
using Xunit;
using Zafiro.Core.FileSystem;

namespace SimpleScript.Tests
{
    public class CompilerTests
    {
        [Fact]
        public void Compile()
        {
            var sut = new Compiler(new Parser(), new FileSystemOperations());
            var script = sut.Compile("Tests\\Root.txt");
            var st = script.Statements.ToList();
            var decl = script.Declarations.ToList();
        }
    }
}