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
        UndeclaredFunctionOrProcedure,
        ReferenceToUninitializedVariable,
        ProcedureAlreadyDeclared,
        ProcedureNameConflictsWithBuiltInFunction
    }
}