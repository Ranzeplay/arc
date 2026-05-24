namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowReturnInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x1b];
}
