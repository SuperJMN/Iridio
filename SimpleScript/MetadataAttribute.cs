using System;

namespace SimpleScript
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MetadataAttribute : Attribute
    {
        public string Key { get; }
        public object Value { get; }

        public MetadataAttribute(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}