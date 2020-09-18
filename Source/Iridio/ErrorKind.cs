namespace Iridio
{
    public enum ErrorKind
    {
        UnableToParse,
        TypeMismatch,
        UndefinedVariable,
        IntegratedFunctionFailure,
        VariableNotFound,
        UndefinedMainFunction,
        BindError,
        UndeclaredFunction,
        ReferenceToUninitializedVariable
    }
}