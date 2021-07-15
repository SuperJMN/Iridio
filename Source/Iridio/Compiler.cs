﻿using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Core;
using Iridio.Parsing;

namespace Iridio
{
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;

        public Compiler(IBinder binder, IParser parser)
        {
            this.parser = parser;
            this.binder = binder;
        }

        public Result<Script, CompilerError> Compile(SourceCode sourceCode)
        {
            var compileResult = parser
                .Parse(sourceCode.Text)
                .MapError(error => (CompilerError) new ParseError(error, Location.From(error.Position, sourceCode)))
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