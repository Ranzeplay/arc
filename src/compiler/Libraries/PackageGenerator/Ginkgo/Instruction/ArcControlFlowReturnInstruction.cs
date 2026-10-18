namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowReturnInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x1b];

    public bool ExpectReturnValue { get; set; }
}
