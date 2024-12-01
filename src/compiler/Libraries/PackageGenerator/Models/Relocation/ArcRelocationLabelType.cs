namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    internal enum ArcRelocationLabelType
    {
        BeginFunction,
        EndFunction,
        BeginIfBlock,
        EndIfBlock,
        BeginIfSubBlock,
        EndIfSubBlock,
        BeginBlock,
        EndBlock,
        BeginLoopBlock,
        EndLoopBlock,
    }
}
