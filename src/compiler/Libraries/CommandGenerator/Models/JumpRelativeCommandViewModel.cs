using Arc.Compiler.Shared.CommandGeneration.Relocation;

namespace Arc.Compiler.CommandGenerator.Models
{
    internal record JumpRelativeCommandViewModel(RelativeRelocator TrueTarget, RelativeRelocator FalseTarget)
    {
    }
}
