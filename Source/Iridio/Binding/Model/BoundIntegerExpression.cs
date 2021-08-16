﻿using Iridio.Core;

namespace Iridio.Binding.Model
{
    public class BoundIntegerExpression : BoundExpression
    {
        public int Value { get; }

        public BoundIntegerExpression(int value, Position position) : base(position)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}