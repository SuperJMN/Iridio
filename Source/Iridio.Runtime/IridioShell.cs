﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Common;
using Iridio.Preprocessing;

namespace Iridio.Runtime
{
    // ReSharper disable once UnusedType.Global
    public class IridioShell : IIridioShell
    {
        private readonly Iridio iridioCore;
        private readonly Preprocessor preprocessor;

        public IridioShell() : this(new HashSet<IFunction>())
        {
        }

        public IridioShell(ISet<IFunction> functions)
        {
            preprocessor = new Preprocessor(new System.IO.Abstractions.FileSystem());
            iridioCore = new Iridio(functions);
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(string path, IDictionary<string, object> initialState)
        {
            var source = preprocessor.Process(path);
            return iridioCore.Run(source, initialState);
        }

        public Task<Result<ExecutionSummary, IridioError>> Run(SourceCode source, IDictionary<string, object> initialState)
        {
            return iridioCore.Run(source, initialState);
        }

        public IObservable<string> Messages => iridioCore.Messages;
    }
}