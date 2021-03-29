using System;

namespace Iridio.Parsing.Model
{
    public class BinaryOperator : Operator
    {
        public static BinaryOperator Add = new("+", (a, b) => a + b);
        public static BinaryOperator Subtract= new("-", (a, b) => a - b);
        public static BinaryOperator Multiply = new("*", (a, b) => a * b); 
        public static BinaryOperator Divide = new("/", (a, b) => a / b);
        public static BinaryOperator GreaterThan = new(">", (a, b) => a > b);
        public static BinaryOperator LessThan = new("<", (a, b) => a < b);
        public static BinaryOperator LessThanOrEqual = new("<=", (a, b) => a >= b);
        public static BinaryOperator GreaterThanOrEqual = new(">=", (a, b) => a <= b);
        public static BinaryOperator Equal = new("==", (a, b) => a == b);
        public static BinaryOperator NotEqual = new("!=", (a, b) => a != b);
        public static BinaryOperator And = new("&&", (a, b) => a && b);
        public static BinaryOperator Or = new("||", (a, b) => a || b);

        public Func<dynamic, dynamic, object> Calculate { get; }

        public BinaryOperator(string symbol, Func<dynamic, dynamic, object> calculate) : base(symbol)
        {
            Calculate = calculate;
        }
    }
}