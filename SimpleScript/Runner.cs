using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public class Runner : IRunner
    {
        private IDictionary<string, object> dict;
        private readonly ISubject<string> messagesSubject = new Subject<string>();
        private readonly Dictionary<string, IFunction> functionsDict;

        public Runner(IEnumerable<IFunction> functions)
        {
            functionsDict = functions.ToDictionary(x => x.Name, x => x);
        }

        public async Task Run(Script script, IDictionary<string, object> variables)
        {
            dict = variables;

            foreach (var sentence in script.Statements)
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
            var parameters = await Task.WhenAll(callExpression.Parameters.Select(Evaluate));
            if (!functionsDict.TryGetValue(callExpression.FuncName, out var func))
            {
                throw new RuntimeException($"Cannot find function {callExpression.FuncName}");
            }

            var invoke = await func.Invoke(parameters);

            return invoke;
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

                    if (!dict.TryGetValue(ie.Identifier, out var value))
                    {
                        throw new RuntimeException($"The variable '{ie.Identifier}' doesn't exist");
                    }

                    return value;
            }

            throw new ArgumentOutOfRangeException($"Unexpected expression of type {assignmentExpression.GetType()}");
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
            messagesSubject.OnNext(msg);
        }

        public IObservable<string> Messages => messagesSubject.AsObservable();
    }
}