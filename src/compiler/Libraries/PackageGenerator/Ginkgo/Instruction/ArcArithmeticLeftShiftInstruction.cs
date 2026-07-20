namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcArithmeticLeftShiftInstruction : ArcArithmeticInstructionBase
{
    public override byte[] Opcode => [0x07];
    
    public ushort Src { get; set; }
    public ushort Amount { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrc { get; set; }
    public bool LastUseAmount { get; set; }
    
    public bool UseDestNext { get; set; }
}
