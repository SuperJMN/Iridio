using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Parsing;
using Iridio.Preprocessing;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    // ReSharper disable once UnusedType.Global
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;
        private readonly IPreprocessor preprocessor;

        public Compiler(IPreprocessor preprocessor, IBinder binder, IParser parser)
        {
            this.parser = parser;
            this.binder = binder;
            this.preprocessor = preprocessor;
        }

        public Either<CompilerError, Script> Compile(string path)
        {
            var processed = preprocessor.Process(path);

            var compileResult = parser
                .Parse(processed.Join())
                .MapLeft(pe => (CompilerError) new ParseError(pe))
                .MapRight(parsed => binder.Bind(parsed)
                    .MapLeft(errors => (CompilerError) new BindError(errors)));

            return compileResult;
        }
    }
}