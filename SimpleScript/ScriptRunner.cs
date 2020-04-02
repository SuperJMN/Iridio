using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SimpleScript.Ast;
using SimpleScript.Ast.Model;
using Superpower;

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

        public Task Run(string source, IDictionary<string, object> variables)
        {
            var tokenizer = Tokenizer.Create().Tokenize(source);
            var script = SimpleParser.SimpleScript.Parse(tokenizer);
            return Run(script, variables);
        }

        public async Task Run(Script script, IDictionary<string, object> variables)
        {
            dict = variables;

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
            var parameters = await Task.WhenAll(callExpression.Parameters.Select(Evaluate));
            var retValue = await instance.ExecuteTask("Execute", parameters);
            return retValue;
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
                    return ReplaceVariables(strExpr.String);
                case NumericExpression numericExpr:
                    return numericExpr.Number;
                case IdentifierExpression ie:
                    return dict[ie.Identifier];
            }

            throw new ArgumentOutOfRangeException();
        }

        private object ReplaceVariables(string str)
        {
            foreach (var variable in dict)
            {
                var token = "{" + variable.Key + "}";
                str = str.Replace(token, variable.Value.ToString());
            }

            return str;
        }

        private void Echo(string msg)
        {
        }
    }
}