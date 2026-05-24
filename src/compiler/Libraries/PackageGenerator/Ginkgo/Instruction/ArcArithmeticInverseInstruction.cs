namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcArithmeticInverseInstruction : ArcArithmeticInstructionBase
{
    public override byte[] Opcode => [0x06];
    
    public ushort Src { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrc { get; set; }
    
    public bool UseDestNext { get; set; }
}
