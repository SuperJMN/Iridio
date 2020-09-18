using System.Collections.Generic;
using Zafiro.Core.Patterns;
using Zafiro.Core.Patterns.Either;

namespace SimpleScript.Tests
{
    internal class ExecutionSummary
    {
        public bool IsSuccess { get; }
        public Dictionary<string, object> Variables { get; }
        public Errors Errors { get; }

        public ExecutionSummary(bool isSuccess, Dictionary<string, object> variables, Errors errors)
        {
            IsSuccess = isSuccess;
            Variables = variables;
            Errors = errors;
        }
    }
}