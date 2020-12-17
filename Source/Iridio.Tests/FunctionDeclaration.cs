using Iridio.Binding;

namespace Iridio.Tests
{
    internal class FunctionDeclaration : IFunctionDeclaration
    {
        public string Name { get; }

        public FunctionDeclaration(string name)
        {
            Name = name;
        }
    }
}