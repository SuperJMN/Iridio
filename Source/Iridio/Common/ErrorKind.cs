namespace Iridio.Common
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
        ReferenceToUninitializedVariable,
        ProcedureAlreadyDeclared
    }
}