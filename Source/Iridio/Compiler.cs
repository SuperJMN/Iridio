using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    public class Compiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;

        public Compiler(IParser parser, IBinder binder)
        {
            this.parser = parser;
            this.binder = binder;
        }

        public Either<Errors, CompiledScript> Compile(string input)
        {
            var compileResult = parser
                .Parse(input)
                .MapLeft(pr => new Errors(ErrorKind.UnableToParse))
                .MapRight(parsed => binder.Bind(parsed));

            return compileResult;
        }
    }
}