﻿using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Iridio.Tokenization;
using Superpower.Model;
using Position = Iridio.Core.Position;

namespace Iridio.Binding.Model
{
    public class Script : IBoundNode
    {
        public IReadOnlyCollection<BoundProcedure> Procedures { get; }

        public Script(IEnumerable<BoundProcedure> procedures)
        {
            Procedures = procedures.ToList();
        }

        public override string ToString()
        {
            return string.Join("\n", Procedures);
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Maybe<Position> Position { get; set; }

        public Token<SimpleToken> Token { get; set; }
    }
}