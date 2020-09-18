using System.Collections.Generic;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;

        public Compiler(IEnumerable<IFunction> functions)
        {
            this.parser = new Parser();
            this.binder = new Binder(new BindingContext(functions));
        }

        public Either<Errors, CompilationUnit> Compile(string input)
        {
            var compileResult = parser
                .Parse(input)
                .MapLeft(pr => new Errors(ErrorKind.UnableToParse))
                .MapRight(parsed => binder.Bind(parsed));

            return compileResult;
        }
    }
}