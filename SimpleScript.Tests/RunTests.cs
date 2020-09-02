using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using SimpleScript.Parsing.Model;
using Xunit;

namespace SimpleScript.Tests
{
    public class RunTests
    {
        [Fact]
        public async Task MyTest()
        {
            MethodInfo inf = typeof(IntTask).GetMethod("Execute");
            var intTask = new IntTask();
            Delegate d = inf.CreateDelegate(intTask);
            var ret =  await (Task<object>) d.Method.Invoke(intTask, new object[] {123});
        }

        //[Fact]
        //public async Task Run()
        //{
        //    IEnumerable<FunctionDeclaration> funcs = new[] { new FunctionDeclaration(typeof(IntTask)), new FunctionDeclaration(typeof(StringTask)), };
        //    var runner = new Runner(funcs);
        //    var compiler = new Compiler(new Parser(), new FileSystemOperations());
        //    var script = compiler.Compile("Root.txt");
        //    var dictionary = new Dictionary<string, object>();
        //    await runner.Run(script, dictionary);
        //}
    }

    public class Function : IFunction
    {
        public Function(string name)
        {
            Name = name;
        }

        public Task<object> Invoke(object[] parameters)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
        public IEnumerable<Argument> Arguments { get; }
        public Type ReturnType { get; }
    }

    public class LambdaFunction<T1, T2, T3> : IFunction
    {
        public LambdaFunction(string name, Func<T1, T2, T3> func)
        {
            Name = name;
            Func = func;
        }

        public async Task<object> Invoke(object[] parameters)
        {
            var result = Func.DynamicInvoke(parameters);
            return (T3)result;
        }

        public string Name { get; }
        public Func<T1, T2, T3> Func { get; }
        public IEnumerable<Argument> Arguments { get; }
        public Type ReturnType { get; }
    }
}