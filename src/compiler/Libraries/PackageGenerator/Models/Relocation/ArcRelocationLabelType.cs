namespace Arc.Compiler.PackageGenerator.Models.Relocation
{
    public enum ArcRelocationLabelType
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
    
    public static class ArcRelocationLabelTypeExtensions
    {
        public static ArcRelocationLabelType GetAntiLabel(this ArcRelocationLabelType type)
        {
            return type switch
            {
                ArcRelocationLabelType.BeginFunction => ArcRelocationLabelType.EndFunction,
                ArcRelocationLabelType.EndFunction => ArcRelocationLabelType.BeginFunction,
                ArcRelocationLabelType.BeginIfBlock => ArcRelocationLabelType.EndIfBlock,
                ArcRelocationLabelType.EndIfBlock => ArcRelocationLabelType.BeginIfBlock,
                ArcRelocationLabelType.BeginIfSubBlock => ArcRelocationLabelType.EndIfSubBlock,
                ArcRelocationLabelType.EndIfSubBlock => ArcRelocationLabelType.BeginIfSubBlock,
                ArcRelocationLabelType.BeginBlock => ArcRelocationLabelType.EndBlock,
                ArcRelocationLabelType.EndBlock => ArcRelocationLabelType.BeginBlock,
                ArcRelocationLabelType.BeginLoopBlock => ArcRelocationLabelType.EndLoopBlock,
                ArcRelocationLabelType.EndLoopBlock => ArcRelocationLabelType.BeginLoopBlock,
                _ => throw new InvalidOperationException()
            };
        }
    }
}
