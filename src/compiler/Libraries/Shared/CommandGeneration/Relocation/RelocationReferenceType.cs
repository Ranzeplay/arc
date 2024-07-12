namespace Arc.Compiler.Shared.CommandGeneration.Relocation
{
    public enum RelocationReferenceType
    {
        FunctionEntrance,
        EndFunction,
        IfEntrance,
        ElifEntrance,
        ElseEntrance,
        EndIf,
        EndElif,
        EndElse,
        WhileEntrance,
        EndWhile,
        LoopEntrance,
        EndLoop,
        DomainEntrance,
        EndDomain
    }
}