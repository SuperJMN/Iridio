using System.Collections.Generic;

namespace SimpleScript.Parsing.Model
{
    public class Header : ISyntax
    {
        public IEnumerable<Declaration> Declarations { get; }

        public Header(IEnumerable<Declaration> declarations)
        {
            Declarations = declarations;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}