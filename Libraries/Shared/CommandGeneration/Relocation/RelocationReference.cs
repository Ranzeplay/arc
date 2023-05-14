namespace Arc.Compiler.Shared.CommandGeneration.Relocation
{
    public record RelocationReference(long CommandLocation, RelocationReferenceType ReferenceType)
    {
    }
}
