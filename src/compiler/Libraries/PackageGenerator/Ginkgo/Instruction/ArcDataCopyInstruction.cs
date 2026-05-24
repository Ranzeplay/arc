namespace Arc.Compiler.PackageGenerator.Ginkgo.Instruction;

public class ArcDataCopyInstruction : ArcDataInstructionBase
{
    public override byte[] Opcode => [0x1f];
    
    public ushort Src { get; set; }
    
    public ushort Dest { get; set; }
    
    public bool LastUseSrc { get; set; }
    
    public bool UseDestNext { get; set; }
}
