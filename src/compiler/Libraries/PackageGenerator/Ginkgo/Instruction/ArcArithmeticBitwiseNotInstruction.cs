namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcArithmeticBitwiseNotInstruction : ArcArithmeticInstructionBase
{
    public override byte[] Opcode => [0x0d];
    
    public ushort Src { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrc { get; set; }
    
    public bool UseDestNext { get; set; }
}
