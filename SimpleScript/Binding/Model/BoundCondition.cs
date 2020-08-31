﻿using SimpleScript.Parsing.Model;

namespace SimpleScript.Binding.Model
{
    public class BoundCondition : BoundStatement
    {
        public BoundExpression Left { get; }
        public BooleanOperator Op { get; }
        public BoundExpression Right { get; }

        public BoundCondition(BoundExpression left, BooleanOperator op, BoundExpression right)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}