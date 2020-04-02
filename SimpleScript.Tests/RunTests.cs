using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleScript;
using SimpleScript.Ast.Model;
using Xunit;

namespace Tests
{
    public class RunTests
    {
        [Fact]
        public async Task Run()
        {
            var sut = new ScriptRunner(new InstanceCreator(), typeof(IntTask).Assembly.ExportedTypes);
            var dictionary = new Dictionary<string, object>();
            await sut.Run("a = IntTask(1);\nb = \"Johnny was a good man\";\nStringTask(\"{b}\");", dictionary);
            await sut.Run("b = \"Johnny was a good man\";", dictionary);
        }
    }
}