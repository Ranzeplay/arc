namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowReturnWithValueInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x1a];
    
    public ushort Src { get; set; }
}
