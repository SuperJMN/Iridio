using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Core;
using Iridio.Parsing;
using Iridio.Preprocessing;

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

        public Result<Script, CompilerError> Compile(string path)
        {
            var preprocessed = preprocessor.Process(path);

            var compileResult = parser
                .Parse(preprocessed.Text)
                .MapError(error => (CompilerError) new ParseError(error, Location.From(error.Position, preprocessed)))
                .Bind(parsed =>
                {
                    return binder
                        .Bind(parsed)
                        .MapError(errors => (CompilerError) new BindError(errors));
                });

            return compileResult;
        }
    }
}