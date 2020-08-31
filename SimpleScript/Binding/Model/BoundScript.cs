﻿using System.Collections.Generic;

namespace SimpleScript.Binding.Model
{
    public class BoundScript : IBoundNode
    {
        public IEnumerable<BoundFunction> Functions { get; }

        public BoundScript(IEnumerable<BoundFunction> functions)
        {
            Functions = functions;
        }

        public override string ToString()
        {
            return string.Join("\n", Functions);
        }

        public void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public interface IBoundNode
    {
        void Accept(IBoundNodeVisitor visitor);
    }
}