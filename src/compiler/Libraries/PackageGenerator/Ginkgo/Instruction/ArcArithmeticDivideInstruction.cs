namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcArithmeticDivideInstruction : ArcArithmeticInstructionBase
{
    public override byte[] Opcode => [0x04];
    
    public ushort SrcA { get; set; }
    public ushort SrcB { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrcA { get; set; }
    public bool LastUseSrcB { get; set; }
    
    public bool UseDestNext { get; set; }
}
