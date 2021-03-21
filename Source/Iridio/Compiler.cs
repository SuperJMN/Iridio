﻿using System.Linq;
using Iridio.Binding;
using Iridio.Binding.Model;
using Iridio.Common;
using Iridio.Parsing;
using Zafiro.Core.FileSystem;
using Zafiro.Core.Patterns.Either;

namespace Iridio
{
    // ReSharper disable once UnusedType.Global
    public class Compiler : ICompiler
    {
        private readonly IParser parser;
        private readonly IBinder binder;
        private readonly Preprocessor preprocessor;

        public Compiler(IFileSystemOperations fileSystemOperations)
        {
            parser = new Parser();
            binder = new Binder(Enumerable.Empty<IFunctionDeclaration>());
            preprocessor = new Preprocessor(fileSystemOperations);
        }

        public Either<CompilerError, Script> Compile(string path)
        {
            var processed = preprocessor.Process(path);

            var compileResult = parser
                .Parse(processed)
                .MapLeft(pe => (CompilerError)new ParseError(pe))
                .MapRight(parsed => binder.Bind(parsed)
                    .MapLeft(errors => (CompilerError)new BindError(errors)));

            return compileResult;
        }
    }

    public class BindError : CompilerError
    {
        public BinderErrors Errors { get; }

        public BindError(BinderErrors errors)
        {
            Errors = errors;
        }
    }

    public class ParseError : CompilerError
    {
        public ParsingError Error { get; }

        public ParseError(ParsingError error)
        {
            Error = error;
        }

        public override string ToString()
        {
            return Error.ToString();
        }
    }

    public abstract class CompilerError
    {
    }
}