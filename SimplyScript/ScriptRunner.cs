using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SimpleScript.Ast;
using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public class ScriptRunner
    {
        private readonly IInstanceBuilder builder;
        private readonly IEnumerable<Type> types;
        private IDictionary<string, object> dict;

        public ScriptRunner(IInstanceBuilder builder, IEnumerable<Type> types)
        {
            this.builder = builder;
            this.types = types;
        }

        public async Task Run(Script script, IDictionary<string, object> variables)
        {
            this.dict = variables;

            foreach (var sentence in script.Sentences)
            {
                await Execute(sentence);
            }
        }

        private async Task Execute(Statement statement)
        {
            switch (statement)
            {
                case EchoStatement echo:
                    Echo(echo.Message);
                    break;
                case AssignmentStatement assignment:
                    await Evaluate(assignment);
                    break;
                case CallStatement call:
                    await Evaluate(call);
                    break;
            }
        }

        private async Task Evaluate(AssignmentStatement assignment)
        {
            var value = await Evaluate(assignment.Expression);
            dict[assignment.Variable] = value;
        }

        private Task<object> Evaluate(CallStatement call)
        {
            return Evaluate(call.Expression);
        }

        private async Task<object> Evaluate(CallExpression callExpression)
        {
            var type = GetFuncType(callExpression.FuncName);
            var instance = builder.Build(type);
            var waitAll = await Task.WhenAll(callExpression.Parameters.Select(Evaluate));
            var p = await instance.ExecuteTask("Execute", waitAll);
            return 123;
        }

        private Type GetFuncType(string name)
        {
            return types.First(type => type.Name == name);
        }

        private async Task<object> Evaluate(Expression assignmentExpression)
        {
            switch (assignmentExpression)
            {
                case CallExpression expression:
                    return await Evaluate(expression);
                case StringExpression strExpr:
                    return strExpr.String;
                case NumericExpression numericExpr:
                    return numericExpr.Number;
            }

            throw new ArgumentOutOfRangeException();
        }

        private void Echo(string msg)
        {
        }
    }

    public interface IInstanceBuilder
    {
        object Build(Type type, params object[] parameters);
    }
}