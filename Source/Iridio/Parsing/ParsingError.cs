﻿namespace Iridio.Parsing
{
    public class ParsingError
    {
        public string Message { get; }

        public ParsingError(string message)
        {
            Message = message;
        }
    }
}