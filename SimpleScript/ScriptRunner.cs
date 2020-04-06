using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using SimpleScript.Ast.Model;

namespace SimpleScript
{
    public class ScriptRunner : IScriptRunner
    {
        private IDictionary<string, object> dict;
        private readonly ISubject<string> messagesSubject = new Subject<string>();
        private readonly Dictionary<string, IFunction> functionsDict;

        public ScriptRunner(IEnumerable<IFunction> functions)
        {
            functionsDict = functions.ToDictionary(x => x.Name, x => x);
        }

        public async Task Run(ScriptSyntax script, IDictionary<string, object> variables)
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
            var parameters = await Task.WhenAll(callExpression.Parameters.Select(Evaluate));
            var func = functionsDict[callExpression.FuncName];
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
            messagesSubject.OnNext(msg);
        }

        public IObservable<string> Messages => messagesSubject.AsObservable();
    }
}