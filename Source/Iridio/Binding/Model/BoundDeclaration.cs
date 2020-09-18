namespace Iridio.Binding.Model
{
    public class BoundDeclaration
    {
        public BoundDeclaration(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
    }
}