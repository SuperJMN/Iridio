using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Core;
using Iridio.Parsing;

namespace Iridio
{
    public class SourceCodeCompiler : ISourceCodeCompiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;

        public SourceCodeCompiler(IBinder binder, IParser parser)
        {
            this.parser = parser;
            this.binder = binder;
        }

        public Result<Script, CompilerError> Compile(SourceCode sourceCode)
        {
            var compileResult = parser
                .Parse(sourceCode.Text)
                .MapError(error => (CompilerError)new ParseError(SourceUnit.From(error.Position, sourceCode), error.Message, sourceCode))
                .Bind(parsed =>
                {
                    return binder
                        .Bind(parsed)
                        .MapError(errors => (CompilerError)new BindError(errors, sourceCode));
                });

            return compileResult;
        }
    }
}