namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcLogicalNotInstruction : ArcLogicalInstructionBase
{
    public override byte[] Opcode => [0x26];
    
    public ushort Src { get; set; }
    public ushort Dest { get; set; }
    
    public bool LastUseSrc { get; set; }
    
    public bool UseDestNext { get; set; }
}
