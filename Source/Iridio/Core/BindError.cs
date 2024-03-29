﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Parsing;

namespace Iridio.Core
{
    public class BindError : CompilerError
    {
        public BinderErrors BinderErrors { get; }

        public BindError(BinderErrors binderErrors, SourceCode sourceCode) : base(sourceCode)
        {
            BinderErrors = binderErrors;
        }

        public override string ToString()
        {
            return string.Join(";", BinderErrors.Select(x => x.ToString()));
        }

        public override IReadOnlyCollection<ErrorItem> Errors => new ReadOnlyCollection<ErrorItem>(BinderErrors
            .Select(x => new ErrorItem(x.ToString(), x.Position.Map(position => SourceUnit.From(position, SourceCode)))).ToList());
    }
}