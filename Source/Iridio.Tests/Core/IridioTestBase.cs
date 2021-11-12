using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio.Binding;
using Iridio.Common;
using Iridio.Parsing;
using Iridio.Runtime;
using Iridio.Tests.Execution;

namespace Iridio.Tests.Core
{
    public class IridioTestBase
    {
        protected static IridioCore CreateSut()
        {
            var functions = new List<IFunction>
            {
                new LambdaFunction<int, int, int>("Sum", (x, y) => x + y),
                new LambdaFunction<Result>("Throw", () => throw new NotSupportedException()),
                new LambdaFunction<Result>("Cancel", () => throw new TaskCanceledException())
            };

            var sut = new IridioCore(new SourceCodeCompiler(new Binder(functions), new Parser()), new Interpreter(functions));
            return sut;
        }


        protected static async Task<Result<ExecutionSummary, IridioError>> Run(SourceCode fromString)
        {
            var iridioCore = CreateSut();
            return await iridioCore.Run(fromString, new Dictionary<string, object>());
        }
    }
}