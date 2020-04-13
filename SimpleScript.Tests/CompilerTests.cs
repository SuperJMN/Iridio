using System.Linq;
using Xunit;

namespace SimpleScript.Tests
{
    public class CompilerTests
    {
        [Fact]
        public void Compile()
        {
            var source = @"
[Requirement:Disk]
[Requirement:WimFile]
a = IntTask(1);
b = ""Johnny was a good man"";
StringTask(""{b}"");
!""c:\myscript.txt"";";

            var sut = new Compiler(new TestFileOperations("[Requirement:Disk]a=3;"), new Parser());
            var script = sut.Compile(source);
            var st = script.Statements.ToList();
            var decl = script.Declarations.ToList();
        }
    }
}