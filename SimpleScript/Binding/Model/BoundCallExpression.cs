using System.Collections.Generic;
using Zafiro.Core.Patterns;

namespace SimpleScript.Binding.Model
{
    public class BoundCallExpression : BoundExpression
    {
        public BoundCallExpression(string name, IEnumerable<BoundExpression> parameters)
        {
            throw new System.NotImplementedException();
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}