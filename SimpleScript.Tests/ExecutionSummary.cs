using System.Collections.Generic;
using Zafiro.Core.Patterns;

namespace SimpleScript.Tests
{
    internal class ExecutionSummary
    {
        public bool IsSuccess { get; }
        public Dictionary<string, object> Variables { get; }
        public ErrorList Errors { get; }

        public ExecutionSummary(bool isSuccess, Dictionary<string, object> variables, ErrorList errors)
        {
            IsSuccess = isSuccess;
            Variables = variables;
            Errors = errors;
        }
    }
}