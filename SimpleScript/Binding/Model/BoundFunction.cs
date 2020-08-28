using SimpleScript.Parsing.Model;

namespace SimpleScript.Binding
{
    internal class BoundFunction
    {
        public Function Func { get; }

        public BoundFunction(Function func)
        {
            Func = func;
        }
    }
}