﻿using System.Collections.Generic;

namespace Iridio.Runtime
{
    public class TypeMismatch : RunError
    {
        public override IEnumerable<string> Items => new[] {ToString()};

        public override string ToString()
        {
            return "Type mismatch";
        }
    }
}