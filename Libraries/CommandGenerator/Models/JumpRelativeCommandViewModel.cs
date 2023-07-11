using Arc.Compiler.Shared.CommandGeneration.Relocation;

namespace Arc.CompilerCommandGenerator.Models
{
    internal record JumpRelativeCommandViewModel(RelativeRelocator TrueTarget, RelativeRelocator FalseTarget)
    {
    }
}
