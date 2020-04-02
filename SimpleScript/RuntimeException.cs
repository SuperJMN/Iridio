using System;

namespace SimpleScript
{
    internal class RuntimeException : ApplicationException
    {
        public RuntimeException(string message) : base(message)
        {
        }
    }
}