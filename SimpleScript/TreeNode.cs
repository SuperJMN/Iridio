using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleScript
{
    internal class TreeNode<T>
    {
        public TreeNode(T value) : this(value, Enumerable.Empty<TreeNode<T>>())
        {
        }

        public TreeNode(T value, IEnumerable<TreeNode<T>> children)
        {
            Value = value;
            Children = children;
        }

        public T Value { get; }
        public IEnumerable<TreeNode<T>> Children { get; }
    }

    public class ErrorList : Collection<Error>
    {
        public ErrorList(ErrorKind unableToParse, string additionalData = null) : base(new List<Error>{ new Error(unableToParse, additionalData)})
        {
        }

        public ErrorList(IEnumerable<Error> items) : base(items.ToList())
        {
        }

        public ErrorList()
        {
        }
    }

    public class Error
    {
        public Error(ErrorKind kind, string additionalData = null)
        {
            ErrorKind = kind;
            AdditionalData = additionalData;
        }

        public ErrorKind ErrorKind { get; }
        public string AdditionalData { get; }

        public override string ToString()
        {
            return $"{nameof(ErrorKind)}: {ErrorKind}, {nameof(AdditionalData)}: {AdditionalData}";
        }
    }

    public enum ErrorKind
    {
        UnableToParse,
        TypeMismatch,
        UndefinedVariable,
        IntegratedFunctionFailure,
        VariableNotFound,
        UndefinedMainFunction,
        BindError,
        UndeclaredFunction
    }
}