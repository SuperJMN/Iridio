namespace SimpleScript.Binding.Model
{
    public class BoundAssignmentStatement : BoundStatement
    {
        public string Variable { get; }
        public BoundExpression Expression { get; }

        public BoundAssignmentStatement(string variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override string ToString()
        {
            return $"\t{Variable} = {Expression};"; 
        }

        public override void Accept(IBoundNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}