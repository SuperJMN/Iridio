using System;
using System.Collections.Generic;
using ScriptParser;
using SimpleScript;
using SimpleScript.Ast.Model;
using Xunit;

namespace Tests
{
    public class RunTests
    {
        [Fact]
        public void Run()
        {
            var sut = new ScriptRunner(new InstanceCreator(), new List<Type>(){ typeof(MyTask)});
            var script = new Script();
            sut.Run(script,);
        }
    }
}