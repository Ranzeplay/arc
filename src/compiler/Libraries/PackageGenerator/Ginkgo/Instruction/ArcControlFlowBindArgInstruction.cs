namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcControlFlowBindArgInstruction : ArcControlFlowInstructionBase
{
    public override byte[] Opcode => [0x18];
    
    public ushort Src { get; set; }
    
    public bool LastUseSrc { get; set; }
}
