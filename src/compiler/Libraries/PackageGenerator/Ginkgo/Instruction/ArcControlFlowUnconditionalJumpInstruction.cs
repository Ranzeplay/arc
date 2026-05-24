using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowUnconditionalJumpInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x15];
    
    public required ArcRelocationTarget Target { get; set; }
}
