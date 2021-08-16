﻿using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundCallStatement : BoundStatement
    {
        public BoundCallExpression Call { get; }

        public BoundCallStatement(BoundCallExpression call, Position position) : base(position)
        {
            Call = call;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}