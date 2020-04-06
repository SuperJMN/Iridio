using System.Collections.Generic;

namespace SimpleScript
{
    public class ScriptFactory : IScriptFactory
    {
        private readonly IScriptParser parser;
        private readonly IScriptRunner runner;
        private readonly IEnumerable<IFunction> functions;

        public ScriptFactory(IScriptParser parser, IScriptRunner runner, IEnumerable<IFunction> functions)
        {
            this.parser = parser;
            this.runner = runner;
            this.functions = functions;
        }

        public Script Load(string source)
        {
            var script = parser.Parse(source);
            return new Script(script, runner, functions);
        }
    }
}