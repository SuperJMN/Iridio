using System.Collections.Generic;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing;
using Zafiro.Core.FileSystem;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;
        private readonly Preprocessor preprocessor;

        public Compiler(IEnumerable<IFunction> functions)
        {
            parser = new Parser();
            binder = new Binder(new BindingContext(functions));
            preprocessor = new Preprocessor(new FileSystemOperations());
        }

        public Either<Errors, CompilationUnit> Compile(string path)
        {
            var processed = preprocessor.Process(path);

            var compileResult = parser
                .Parse(processed)
                .MapLeft(pr => new Errors(ErrorKind.UnableToParse))
                .MapRight(parsed => binder.Bind(parsed));

            return compileResult;
        }
    }
}