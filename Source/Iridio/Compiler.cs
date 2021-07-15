using CSharpFunctionalExtensions;
using Iridio.Binding.Model;
using Iridio.Core;
using Iridio.Preprocessing;

namespace Iridio
{
    internal class Compiler : IPathBasedCompiler
    {
        private readonly IPreprocessor preprocessor;
        private readonly ISourceCodeCompiler compiler;

        public Compiler(IPreprocessor preprocessor, ISourceCodeCompiler compiler)
        {
            this.preprocessor = preprocessor;
            this.compiler = compiler;
        }

        public Result<Script, CompilerError> Compile(string path)
        {
            var input = preprocessor.Process(path);
            return compiler.Compile(input);
        }
    }
}