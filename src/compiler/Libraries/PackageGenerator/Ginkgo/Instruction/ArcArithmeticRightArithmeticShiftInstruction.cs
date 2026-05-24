namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcArithmeticRightArithmeticShiftInstruction : ArcArithmeticInstructionBase
{
    public override byte[] Opcode => [0x08];
    
    public ushort Src { get; set; }
    public ushort Amount { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrc { get; set; }
    public bool LastUseAmount { get; set; }
    
    public bool UseDestNext { get; set; }
}
