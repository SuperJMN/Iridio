using System.Collections.Generic;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Parsing;
using Iridio.Preprocessor;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    // ReSharper disable once UnusedType.Global
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;
        private readonly NewPreprocessor preprocessor;

        public Compiler(IEnumerable<IFunctionDeclaration> functionDeclarations)
        {
            parser = new Parser();
            binder = new Binder(functionDeclarations);
            preprocessor = new NewPreprocessor(new TextFileFactory(), new DirectoryContext());
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