namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowLabelInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x14];
    public Arc.Compiler.PackageGenerator.Models.Relocation.ArcRelocationLabel? Label { get; init; }
}
