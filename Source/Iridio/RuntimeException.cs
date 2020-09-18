using System;

namespace Iridio
{
    internal class RuntimeException : ApplicationException
    {
        public RuntimeException(string message) : base(message)
        {
        }
    }
}